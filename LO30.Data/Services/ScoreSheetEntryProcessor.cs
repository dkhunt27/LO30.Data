using LO30.Data.Contexts;
using LO30.Data.Models;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LO30.Data.Services
{
  public class ScoreSheetEntryProcessor
  {
    DateTime _first = DateTime.Now;
    DateTime _last = DateTime.Now;
    TimeSpan _diffFromFirst = new TimeSpan();
    TimeSpan _diffFromLast = new TimeSpan();

    private LogWriter _logger;
    private LO30Context _context;
    private LO30ContextService _lo30ContextService;
    private TimeService _timeService;

    public ScoreSheetEntryProcessor(LogWriter logger, LO30Context context)
    {
      _logger = logger;
      _context = context;
      _lo30ContextService = new LO30ContextService(context);
      _timeService = new TimeService();
    }


    public List<ForWebTeamStanding> DeriveWebTeamStandings(List<TeamStanding> teamStandings)
    {
      var newData = new List<ForWebTeamStanding>();

      foreach (var item in teamStandings)
      {
        float winPct = 0;
        if (item.Games > 0)
        {
          winPct = (float)item.Wins / (float)item.Games;
        }
        var stat = new ForWebTeamStanding()
        {
          TID = item.TeamId,
          PFS = item.Playoffs,
          SID = item.Team.SeasonId,
          Div = item.Team.Division.DivisionLongName,
          Team = item.Team.TeamNameLong,
          Rank = item.Rank,
          GP = item.Games,
          W = item.Wins,
          L = item.Losses,
          T = item.Ties,
          PTS = item.Points,
          GF = item.GoalsFor,
          GA = item.GoalsAgainst,
          PIM = item.PenaltyMinutes,
          S = item.Subs,
          WPCT = winPct
        };

        if (stat.TID == 0)
        {
          _logger.Write(string.Format("DeriveWebTeamStandings: Warning ForWebTeamStanding has tid=0 Team:{0}", stat.Team));
        }
        newData.Add(stat);
      }

      return newData;
    }

    public List<PlayerStatGame> DerivePlayerGameStats(List<ScoreSheetEntryProcessedGoal> scoreSheetEntryProcessedGoals, List<ScoreSheetEntryProcessedPenalty> scoreSheetEntryPenaltiesProcessed, List<GameRoster> gameRosters)
    {
      var playerGameStats = new List<PlayerStatGame>();

      try
      {
        #region process each gameRoster...using game rosters because not every one will be on the score sheets for points/pims
        foreach (var gameRoster in gameRosters)
        {
          #region determine key fields (gameId, teamId, playerId, playerStatTypeId, sub, etc)
          var gameId = gameRoster.GameId;
          var teamId = gameRoster.TeamId;
          var playoffs = gameRoster.Game.Playoffs;
          var sub = gameRoster.Sub;
          var playerId = gameRoster.PlayerId;
          var line = gameRoster.Line;
          var position = gameRoster.Position;
          var seasonId = gameRoster.SeasonId;
          #endregion

          // get score sheet entries for this player, for this game...if exists...if he had no points or pim, it won't exist
          int goals = 0;
          int assists = 0;
          int penaltyMinutes = 0;
          int powerPlayGoals = 0;
          int shortHandedGoals = 0;
          int gameWinningGoals = 0;

          #region process all score sheet entries for this specific game/player
          var scoreSheetEntryGoals = scoreSheetEntryProcessedGoals
                                      .Where(x =>
                                        x.GameId == gameId &&
                                        (
                                          x.GoalPlayerId == playerId ||
                                          x.Assist1PlayerId == playerId ||
                                          x.Assist2PlayerId == playerId ||
                                          x.Assist3PlayerId == playerId
                                        )
                                      )
                                      .ToList();

          foreach (var scoreSheetEntryGoal in scoreSheetEntryGoals)
          {
            if (scoreSheetEntryGoal.GoalPlayerId == playerId)
            {
              goals++;

              if (scoreSheetEntryGoal.ShortHandedGoal)
              {
                shortHandedGoals++;
              }
              else if (scoreSheetEntryGoal.PowerPlayGoal)
              {
                powerPlayGoals++;
              }

              // can be (shorthanded or powerplay) and a game winner...so not else if here, just if
              if (scoreSheetEntryGoal.GameWinningGoal)
              {
                gameWinningGoals++;
              }

            }
            else
            {
              // the score sheet entry must match this player on a goal or assist
              assists++;
            }

          }
          #endregion

          #region process all score sheet entry penalties for this specific game/player
          var scoreSheetEntryPenalties = scoreSheetEntryPenaltiesProcessed
                                                  .Where(x => x.GameId == gameId && x.PlayerId == playerId)
                                                  .ToList();

          foreach (var scoreSheetEntryPenalty in scoreSheetEntryPenalties)
          {
            penaltyMinutes = penaltyMinutes + scoreSheetEntryPenalty.PenaltyMinutes;
          }
          #endregion

          playerGameStats.Add(new PlayerStatGame(
                                        pid: playerId,
                                        gid: gameId,

                                        sid: seasonId,
                                        tid: teamId,
                                        pfs: playoffs,
                                        line: line,
                                        pos: position,
                                        sub: sub,

                                        g: goals,
                                        a: assists,
                                        p: goals + assists,
                                        ppg: powerPlayGoals,
                                        shg: shortHandedGoals,
                                        gwg: gameWinningGoals,
                                        pim: penaltyMinutes
                                        )
                                );
        }
        #endregion

        return playerGameStats;
      }
      catch (Exception ex)
      {
        _logger.Write(ex);
        throw ex;
      }
    }

    public List<PlayerStatTeam> DerivePlayerTeamStats(List<PlayerStatGame> playerGameStats)
    {
      var playerTeamStats = new List<PlayerStatTeam>();

      var summedStats = playerGameStats
              .GroupBy(x => new { x.PlayerId, x.SeasonId, x.Playoffs, x.Sub, TeamIdPlayingFor = x.TeamId, x.Line, x.Position })
              .Select(grp => new
              {
                PlayerId = grp.Key.PlayerId,
                SeasonId = grp.Key.SeasonId,
                Playoffs = grp.Key.Playoffs,
                Sub = grp.Key.Sub,
                TeamIdPlayingFor = grp.Key.TeamIdPlayingFor,
                Line = grp.Key.Line,
                Position = grp.Key.Position,

                Games = grp.Count(),
                Goals = grp.Sum(x => x.Goals),
                Assists = grp.Sum(x => x.Assists),
                Points = grp.Sum(x => x.Points),
                PenaltyMinutes = grp.Sum(x => x.PenaltyMinutes),
                ShortHandedGoals = grp.Sum(x => x.ShortHandedGoals),
                PowerPlayGoals = grp.Sum(x => x.PowerPlayGoals),
                GameWinningGoals = grp.Sum(x => x.GameWinningGoals)
              })
              .ToList();

      foreach (var stat in summedStats)
      {
        playerTeamStats.Add(new PlayerStatTeam(
                                    pid: stat.PlayerId,
                                    sid: stat.SeasonId,
                                    pfs: stat.Playoffs,
                                    sub: stat.Sub,
                                    tid: stat.TeamIdPlayingFor,
                                    line: stat.Line,
                                    pos: stat.Position,


                                    games: stat.Games,
                                    g: stat.Goals,
                                    a: stat.Assists,
                                    p: stat.Points,
                                    ppg: stat.PowerPlayGoals,
                                    shg: stat.ShortHandedGoals,
                                    gwg: stat.GameWinningGoals,
                                    pim: stat.PenaltyMinutes
                                    )
                              );
      }

      return playerTeamStats;
    }

    public List<PlayerStatSeason> DerivePlayerSeasonStats(List<PlayerStatTeam> playerTeamStats)
    {
      var playerSeasonStats = new List<PlayerStatSeason>();

      var summedStats = playerTeamStats
              .GroupBy(x => new { x.PlayerId, x.SeasonId, x.Playoffs, x.Sub })
              .Select(grp => new
              {
                PlayerId = grp.Key.PlayerId,
                SeasonId = grp.Key.SeasonId,
                Playoffs = grp.Key.Playoffs,
                Sub = grp.Key.Sub,

                Games = grp.Sum(x => x.Games),
                Goals = grp.Sum(x => x.Goals),
                Assists = grp.Sum(x => x.Assists),
                Points = grp.Sum(x => x.Points),
                PenaltyMinutes = grp.Sum(x => x.PenaltyMinutes),
                ShortHandedGoals = grp.Sum(x => x.ShortHandedGoals),
                PowerPlayGoals = grp.Sum(x => x.PowerPlayGoals),
                GameWinningGoals = grp.Sum(x => x.GameWinningGoals)
              })
              .ToList();

      foreach (var stat in summedStats)
      {
        playerSeasonStats.Add(new PlayerStatSeason(
                                    pid: stat.PlayerId,
                                    sid: stat.SeasonId,
                                    pfs: stat.Playoffs,
                                    sub: stat.Sub,

                                    games: stat.Games,
                                    g: stat.Goals,
                                    a: stat.Assists,
                                    p: stat.Points,
                                    ppg: stat.PowerPlayGoals,
                                    shg: stat.ShortHandedGoals,
                                    gwg: stat.GameWinningGoals,
                                    pim: stat.PenaltyMinutes
                                    )
                              );
      }

      return playerSeasonStats;
    }

    public List<ForWebPlayerStat> DeriveWebPlayerStats(List<PlayerStatTeam> playerTeamStats, List<PlayerRating> playerRatings)
    {
      var newData = new List<ForWebPlayerStat>();

      foreach (var item in playerTeamStats)
      {
        var playerName = item.Player.FirstName + " " + item.Player.LastName;
        if (!string.IsNullOrWhiteSpace(item.Player.Suffix))
        {
          playerName = playerName + " " + item.Player.Suffix;
        }

        var stat = new ForWebPlayerStat()
        {
          PID = item.PlayerId,
          TID = item.TeamId,
          PFS = item.Playoffs,
          SID = item.SeasonId,
          Player = playerName,
          Team = item.Team.TeamNameLong,
          Sub = item.Sub == true ? "Y" : "N",
          Pos = item.Position,
          Line = item.Line,
          GP = item.Games,
          G = item.Goals,
          A = item.Assists,
          P = item.Points,
          PPG = item.PowerPlayGoals,
          SHG = item.ShortHandedGoals,
          GWG = item.GameWinningGoals,
          PIM = item.PenaltyMinutes
        };

        if (stat.PID == 0 && stat.SID == 0 && stat.TID == 0)
        {
          _logger.Write(string.Format("DeriveWebPlayerStats: Warning ForWebPlayerStat has ids of 0,0,0 Player:{0}, Team:{1}, Sub:{2}", stat.Player, stat.Team, stat.Sub));
        }
        newData.Add(stat);
      }

      return newData;
    }

    public List<GoalieStatGame> DeriveGoalieGameStats(List<GameOutcome> gameOutcomes, List<GameRoster> gameRostersGoalies)
    {
      var goalieGameStats = new List<GoalieStatGame>();

      try
      {
        #region process each gameRoster...using game rosters because not every one will be on the score sheets for points/pims
        foreach (var gameRoster in gameRostersGoalies)
        {
          #region determine key fields (gameId, teamId, playerId, playerStatTypeId, sub, etc)
          var gameId = gameRoster.GameId;
          var teamId = gameRoster.TeamId;
          var playoffs = gameRoster.Game.Playoffs;
          var sub = gameRoster.Sub;
          var playerId = gameRoster.PlayerId;
          var seasonId = gameRoster.SeasonId;
          #endregion

          // sanity check...make sure there is only 1 goalie for each team for each game
          var check = gameRostersGoalies.Where(x => x.GameId == gameId && x.TeamId == teamId && x.Goalie == true).ToList();

          // remove if goal "subbed" for himself because he forgot his jersey

          if (check.Count < 1 || check.Count > 1)
          {
            throw new ArgumentNullException("gameRosterGoalies", "Every GameRoster must have 1 and only 1 goalie GameId:" + gameId + " TeamId:" + teamId + " Goalie Count:" + check.Count);
          }

          // get score sheet entries for this player, for this game...if exists...if he had no points or pim, it won't exist
          int goalAgainst = 0;
          int shutOuts = 0;
          int wins = 0;

          var gameOutcome = gameOutcomes.Where(x => x.GameId == gameId && x.TeamId == teamId).FirstOrDefault();

          if (gameOutcome != null)
          {
            //throw new ArgumentNullException("gameOutcome", "gameOutcome not found for gameId:" + gameId + " teamId:" + teamId);

            goalAgainst = gameOutcome.GoalsAgainst;
            shutOuts = gameOutcome.GoalsAgainst == 0 ? 1 : 0;
            wins = gameOutcome.Outcome == "W" ? 1 : 0;
          }

          goalieGameStats.Add(new GoalieStatGame(
                                        pid: playerId,
                                        gid: gameId,

                                        sid: seasonId,
                                        tid: teamId,
                                        pfs: playoffs,
                                        sub: sub,

                                        ga: goalAgainst,
                                        so: shutOuts,
                                        w: wins
                                        )
                                );
        }
        #endregion

        return goalieGameStats;
      }
      catch (Exception ex)
      {
        _logger.Write(ex);
        throw ex;
      }
    }

    public List<GoalieStatTeam> DeriveGoalieTeamStats(List<GoalieStatGame> goalieGameStats)
    {
      var goalieTeamStats = new List<GoalieStatTeam>();

      var summedStats = goalieGameStats
              .GroupBy(x => new { x.PlayerId, x.SeasonId, x.Playoffs, x.Sub, TeamIdPlayingFor = x.TeamId })
              .Select(grp => new
              {
                PlayerId = grp.Key.PlayerId,
                SeasonId = grp.Key.SeasonId,
                Playoffs = grp.Key.Playoffs,
                Sub = grp.Key.Sub,
                TeamIdPlayingFor = grp.Key.TeamIdPlayingFor,

                Games = grp.Count(),
                GoalsAgainst = grp.Sum(x => x.GoalsAgainst),
                Shutouts = grp.Sum(x => x.Shutouts),
                Wins = grp.Sum(x => x.Wins)
              })
              .ToList();

      foreach (var stat in summedStats)
      {
        goalieTeamStats.Add(new GoalieStatTeam(
                                    pid: stat.PlayerId,
                                    sid: stat.SeasonId,
                                    pfs: stat.Playoffs,
                                    sub: stat.Sub,
                                    tid: stat.TeamIdPlayingFor,

                                    games: stat.Games,
                                    ga: stat.GoalsAgainst,
                                    so: stat.Shutouts,
                                    w: stat.Wins
                                    )
                              );
      }

      return goalieTeamStats;
    }

    public List<GoalieStatSeason> DeriveGoalieSeasonStats(List<GoalieStatTeam> goalieTeamStats)
    {
      var goalieSeasonStats = new List<GoalieStatSeason>();

      var summedStats = goalieTeamStats
              .GroupBy(x => new { x.PlayerId, x.SeasonId, x.Playoffs, x.Sub })
              .Select(grp => new
              {
                PlayerId = grp.Key.PlayerId,
                SeasonId = grp.Key.SeasonId,
                Playoffs = grp.Key.Playoffs,
                Sub = grp.Key.Sub,

                Games = grp.Sum(x => x.Games),
                GoalsAgainst = grp.Sum(x => x.GoalsAgainst),
                Shutouts = grp.Sum(x => x.Shutouts),
                Wins = grp.Sum(x => x.Wins)
              })
              .ToList();

      foreach (var stat in summedStats)
      {
        goalieSeasonStats.Add(new GoalieStatSeason(
                                    pid: stat.PlayerId,
                                    sid: stat.SeasonId,
                                    pfs: stat.Playoffs,
                                    sub: stat.Sub,

                                    games: stat.Games,
                                    ga: stat.GoalsAgainst,
                                    so: stat.Shutouts,
                                    w: stat.Wins
                                    )
                              );
      }

      return goalieSeasonStats;
    }

    public List<ForWebGoalieStat> DeriveWebGoalieStats(List<GoalieStatTeam> goalieTeamStats)
    {
      var newData = new List<ForWebGoalieStat>();

      foreach (var item in goalieTeamStats)
      {
        var playerName = item.Player.FirstName + " " + item.Player.LastName;
        if (!string.IsNullOrWhiteSpace(item.Player.Suffix))
        {
          playerName = playerName + " " + item.Player.Suffix;
        }

        newData.Add(new ForWebGoalieStat()
        {
          TID = item.TeamId,
          PID = item.PlayerId,
          PFS = item.Playoffs,
          SID = item.SeasonId,
          Player = playerName,
          Team = item.Team.TeamNameLong,
          Sub = item.Sub == true ? "Y" : "N",
          GP = item.Games,
          GA = item.GoalsAgainst,
          GAA = item.GoalsAgainstAverage,
          SO = item.Shutouts,
          W = item.Wins
        });
      }

      return newData;
    }

    private List<ScoreSheetEntryProcessedGoal> DeriveScoreSheetEntryProcessedGoals(List<ScoreSheetEntryGoal> scoreSheetEntryGoals, List<GameTeam> gameTeams, List<GameRoster> gameRosters)
    {
      var newData = new List<ScoreSheetEntryProcessedGoal>();

      foreach (var scoreSheetEntryGoal in scoreSheetEntryGoals)
      {
        var gameId = scoreSheetEntryGoal.GameId;

        // look up the home and away season team id
        // TODO..do this once per game, not per score sheet entry
        var homeGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == true).FirstOrDefault();
        var awayGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == false).FirstOrDefault();

        // lookup game rosters
        // TODO..do this once per game, not per score sheet entry
        var homeTeamRoster = gameRosters.Where(x => x.SeasonId == homeGameTeam.SeasonId && x.TeamId == homeGameTeam.TeamId && x.GameId == gameId).ToList();
        var awayTeamRoster = gameRosters.Where(x => x.SeasonId == awayGameTeam.SeasonId && x.TeamId == awayGameTeam.TeamId && x.GameId == gameId).ToList();

        var homeTeamFlag = scoreSheetEntryGoal.HomeTeam;
        var goalPlayerNumber = scoreSheetEntryGoal.Goal;
        var assist1PlayerNumber = scoreSheetEntryGoal.Assist1;
        var assist2PlayerNumber = scoreSheetEntryGoal.Assist2;
        var assist3PlayerNumber = scoreSheetEntryGoal.Assist3;

        var gameRosterToUse = awayTeamRoster;
        var gameTeamToUse = awayGameTeam;
        if (homeTeamFlag)
        {
          gameRosterToUse = homeTeamRoster;
          gameTeamToUse = homeGameTeam;
        }

        // lookup player ids
        var goalPlayerId = ConvertPlayerNumberIntoPlayer(gameRosterToUse, goalPlayerNumber);
        var assist1PlayerId = ConvertPlayerNumberIntoPlayer(gameRosterToUse, assist1PlayerNumber);
        var assist2PlayerId = ConvertPlayerNumberIntoPlayer(gameRosterToUse, assist2PlayerNumber);
        var assist3PlayerId = ConvertPlayerNumberIntoPlayer(gameRosterToUse, assist3PlayerNumber);

        // determine type goal
        // TODO improve this logic
        bool shortHandedGoal = scoreSheetEntryGoal.ShortHandedPowerPlay == "SH" ? true : false;
        bool powerPlayGoal = scoreSheetEntryGoal.ShortHandedPowerPlay == "PP" ? true : false;
        bool gameWinningGoal = false;

        newData.Add(new ScoreSheetEntryProcessedGoal(
                          ssegid: scoreSheetEntryGoal.ScoreSheetEntryGoalId,

                          sid: gameTeamToUse.SeasonId,
                          tid: gameTeamToUse.TeamId,
                          gid: scoreSheetEntryGoal.GameId,

                          per: scoreSheetEntryGoal.Period,
                          ht: scoreSheetEntryGoal.HomeTeam,
                          time: scoreSheetEntryGoal.TimeRemaining,

                          gpid: Convert.ToInt32(goalPlayerId),
                          a1pid: assist1PlayerId,
                          a2pid: assist2PlayerId,
                          a3pid: assist3PlayerId,

                          shg: shortHandedGoal,
                          ppg: powerPlayGoal,
                          gwg: gameWinningGoal,

                          upd: DateTime.Now
                  ));
      }

      return newData;
    }

    private List<ScoreSheetEntryProcessedPenalty> DeriveScoreSheetEntryProcessedPenalties(List<ScoreSheetEntryPenalty> scoreSheetEntryPenalties, List<GameTeam> gameTeams, List<GameRoster> gameRosters, List<Penalty> penalties)
    {
      var newData = new List<ScoreSheetEntryProcessedPenalty>();

      foreach (var scoreSheetEntryPenalty in scoreSheetEntryPenalties)
      {
        var gameId = scoreSheetEntryPenalty.GameId;

        // look up the home and away team id
        // TODO..do this once per game, not per score sheet entry
        var homeGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == true).FirstOrDefault();
        var awayGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == false).FirstOrDefault();

        // lookup game rosters
        var homeTeamRoster = gameRosters.Where(x => x.GameId == gameId && x.TeamId == homeGameTeam.TeamId).ToList();
        var awayTeamRoster = gameRosters.Where(x => x.GameId == gameId && x.TeamId == awayGameTeam.TeamId).ToList();

        var homeTeamFlag = scoreSheetEntryPenalty.HomeTeam;
        var playerNumber = scoreSheetEntryPenalty.Player;

        var gameRosterToUse = awayTeamRoster;
        var gameTeamToUse = awayGameTeam;
        if (homeTeamFlag)
        {
          gameRosterToUse = homeTeamRoster;
          gameTeamToUse = homeGameTeam;
        }

        // lookup player id
        var playerId = ConvertPlayerNumberIntoPlayer(gameRosterToUse, playerNumber.ToString());

        // lookup penalty
        var penaltyId = penalties.Where(x => x.PenaltyCode == scoreSheetEntryPenalty.PenaltyCode).FirstOrDefault().PenaltyId;

        newData.Add(new ScoreSheetEntryProcessedPenalty(
                          ssepid: scoreSheetEntryPenalty.ScoreSheetEntryPenaltyId,

                          sid: gameTeamToUse.SeasonId,
                          tid: gameTeamToUse.TeamId,
                          gid: scoreSheetEntryPenalty.GameId,

                          per: scoreSheetEntryPenalty.Period,
                          ht: scoreSheetEntryPenalty.HomeTeam,
                          time: scoreSheetEntryPenalty.TimeRemaining,

                          playid: Convert.ToInt32(playerId),
                          penid: penaltyId,
                          pim: scoreSheetEntryPenalty.PenaltyMinutes,

                          upd: DateTime.Now
                  ));
      }

      return newData;
    }

    private List<ScoreSheetEntryProcessedSub> DeriveScoreSheetEntryProcessedSubs(List<ScoreSheetEntrySub> scoreSheetEntrySubs, List<Game> games, List<GameTeam> gameTeams, List<TeamRoster> teamRosters, List<Player> players)
    {
      var newData = new List<ScoreSheetEntryProcessedSub>();

      foreach (var scoreSheetEntrySub in scoreSheetEntrySubs)
      {
        var gameId = scoreSheetEntrySub.GameId;

        var game = games.Where(x => x.GameId == gameId).FirstOrDefault();
        var gameDateYYYYMMDD = _timeService.ConvertDateTimeIntoYYYYMMDD(game.GameDateTime, ifNullReturnMax: false);

        // look up the home and away season team id
        // TODO..do this once per game, not per score sheet entry
        var homeGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == true).FirstOrDefault();
        var awayGameTeam = gameTeams.Where(gt => gt.GameId == gameId && gt.HomeTeam == false).FirstOrDefault();

        // lookup team rosters
        var homeTeamRoster = teamRosters.Where(x => x.SeasonId == homeGameTeam.SeasonId && x.TeamId == homeGameTeam.TeamId && x.StartYYYYMMDD <= gameDateYYYYMMDD && x.EndYYYYMMDD >= gameDateYYYYMMDD).ToList();
        var awayTeamRoster = teamRosters.Where(x => x.SeasonId == awayGameTeam.SeasonId && x.TeamId == awayGameTeam.TeamId && x.StartYYYYMMDD <= gameDateYYYYMMDD && x.EndYYYYMMDD >= gameDateYYYYMMDD).ToList();

        var homeTeamFlag = scoreSheetEntrySub.HomeTeam;
        var jerseyNumber = scoreSheetEntrySub.JerseyNumber;
        var subPlayerId = scoreSheetEntrySub.SubPlayerId;
        var subbingForPlayerId = scoreSheetEntrySub.SubbingForPlayerId;

        var teamRosterToUse = awayTeamRoster;
        var gameTeamToUse = awayGameTeam;
        if (homeTeamFlag)
        {
          teamRosterToUse = homeTeamRoster;
          gameTeamToUse = homeGameTeam;
        }

        // lookup player ids

        // make sure the subbing for is on the team roster
        var foundSubbingForPlayer = teamRosterToUse.Where(x => x.PlayerId == subbingForPlayerId).FirstOrDefault();
        var foundSubPlayer = players.Where(x => x.PlayerId == subPlayerId).FirstOrDefault();

        if (foundSubbingForPlayer == null || foundSubPlayer == null)
        {
          // todo handle bad data
        }
        else
        {
          newData.Add(new ScoreSheetEntryProcessedSub(
                  ssesid: scoreSheetEntrySub.ScoreSheetEntrySubId,

                  sid: gameTeamToUse.SeasonId,
                  tid: gameTeamToUse.TeamId,
                  gid: gameId,
                  ht: homeTeamFlag,

                  spid: subPlayerId,
                  sfpid: subbingForPlayerId,
                  jer: jerseyNumber,

                  upd: DateTime.Now
          ));
        }
      }

      return newData;
    }

    public List<ScoreSheetEntryEvent> DeriveScoreSheetEntryEvents(List<ScoreSheetEntryProcessedGoal> scoreSheetEntryGoals, List<ScoreSheetEntryProcessedPenalty> scoreSheetEntryPenalties)
    {
      // put all the goals and penalties in order
      List<ScoreSheetEntryEvent> sseEvents = new List<ScoreSheetEntryEvent>();

      // add all the goals to the events
      foreach (var sseGoal in scoreSheetEntryGoals)
      {
        sseEvents.Add(new ScoreSheetEntryEvent()
        {
          GameId = sseGoal.GameId,
          TimeElapsed = sseGoal.TimeElapsed,
          EventType = ScoreSheetEntryEventType.Goal,
          ScoreSheetEntryGoalId = sseGoal.ScoreSheetEntryGoalId,
          ScoreSheetEntryPenaltyId = null,
          HomeTeam = sseGoal.HomeTeam,
          MatchPenalty = false
        }
        );
      }

      // add all the penalties to the events
      foreach (var ssePenalty in scoreSheetEntryPenalties)
      {
        sseEvents.Add(new ScoreSheetEntryEvent()
        {
          GameId = ssePenalty.GameId,
          TimeElapsed = ssePenalty.TimeElapsed,
          EventType = ScoreSheetEntryEventType.PenaltyStart,
          ScoreSheetEntryGoalId = null,
          ScoreSheetEntryPenaltyId = ssePenalty.ScoreSheetEntryPenaltyId,
          HomeTeam = ssePenalty.HomeTeam,
          MatchPenalty = ssePenalty.PenaltyMinutes == 5 ? true : false
        }
        );

        sseEvents.Add(new ScoreSheetEntryEvent()
        {
          GameId = ssePenalty.GameId,
          TimeElapsed = ssePenalty.TimeElapsed.Add(new TimeSpan(0, ssePenalty.PenaltyMinutes, 0)),
          EventType = ScoreSheetEntryEventType.PenaltyEnd,
          ScoreSheetEntryGoalId = null,
          ScoreSheetEntryPenaltyId = ssePenalty.ScoreSheetEntryPenaltyId,
          HomeTeam = ssePenalty.HomeTeam,
          MatchPenalty = ssePenalty.PenaltyMinutes == 5 ? true : false
        }
        );
      }

      // VERY IMPORTANT, order the events by time elapsed then event type
      sseEvents = sseEvents.OrderBy(x => x.TimeElapsed).ThenBy(x => x.EventType).ToList();

      return sseEvents;
    }

    public List<ScoreSheetEntryGoalType> ProcessOneGameScoreSheetEntryEvents(List<ScoreSheetEntryEvent> scoreSheetEntryEvents)
    {
      List<ScoreSheetEntryGoalType> results = new List<ScoreSheetEntryGoalType>();

      List<ScoreSheetEntryEvent> homeTeamPenaltyBox = new List<ScoreSheetEntryEvent>();
      List<ScoreSheetEntryEvent> awayTeamPenaltyBox = new List<ScoreSheetEntryEvent>();
      ScoreSheetEntryEvent leavesPenaltyBox;
      ScoreSheetEntryEvent eventToRemove;

      // loop through the events and see if any goals where scored during the penalties
      while (scoreSheetEntryEvents.Count > 0)
      {
        var sseEvent = scoreSheetEntryEvents[0];

        var penaltyBoxToUse = awayTeamPenaltyBox;
        if (sseEvent.HomeTeam)
        {
          penaltyBoxToUse = homeTeamPenaltyBox;
        }

        switch (sseEvent.EventType)
        {
          case ScoreSheetEntryEventType.PenaltyEnd:
            // the penalty ended before any goal was scored...
            // remove person from penalty box

            leavesPenaltyBox = penaltyBoxToUse.Single(x => x.ScoreSheetEntryPenaltyId == sseEvent.ScoreSheetEntryPenaltyId);
            penaltyBoxToUse.Remove(leavesPenaltyBox);

            // remove event since it has been handled
            eventToRemove = scoreSheetEntryEvents.Single(x => x.ScoreSheetEntryPenaltyId == sseEvent.ScoreSheetEntryPenaltyId && x.EventType == sseEvent.EventType);
            scoreSheetEntryEvents.Remove(eventToRemove);
            break;
          case ScoreSheetEntryEventType.PenaltyStart:
            // a penalty occurred, add person to penalty box

            penaltyBoxToUse.Add(sseEvent);

            // remove event since it has been handled
            eventToRemove = scoreSheetEntryEvents.Single(x => x.ScoreSheetEntryPenaltyId == sseEvent.ScoreSheetEntryPenaltyId && x.EventType == sseEvent.EventType);
            scoreSheetEntryEvents.Remove(eventToRemove);
            break;
          case ScoreSheetEntryEventType.Goal:
            // if no one in the penalty box, then its just a regular goal
            if (homeTeamPenaltyBox.Count == 0 && awayTeamPenaltyBox.Count == 0)
            {
              // if no one in the penalty box, then its just a regular goal
              // do nothing
            }
            else if (homeTeamPenaltyBox.Count == awayTeamPenaltyBox.Count)
            {
              // there are the same number of people in the penalty box
              // so the goal was even strength
              // do nothing
            }
            else if (homeTeamPenaltyBox.Count < awayTeamPenaltyBox.Count)
            {
              // the home team is on the power play
              if (sseEvent.HomeTeam)
              {
                // home team scored...on the power play
                results.Add(new ScoreSheetEntryGoalType()
                {
                  ScoreSheetEntryGoalId = Convert.ToInt32(sseEvent.ScoreSheetEntryGoalId),
                  PowerPlayGoal = true,
                  ShortHandedGoal = false,
                  GameWinningGoal = false
                });

                //release person from away team penalty box (but not if match penalty)
                leavesPenaltyBox = awayTeamPenaltyBox.Where(x => x.MatchPenalty == false).OrderBy(x => x.TimeElapsed).FirstOrDefault();
                if (leavesPenaltyBox != null)
                {
                  awayTeamPenaltyBox.Remove(leavesPenaltyBox);

                  // also remove the penalty end event
                  eventToRemove = scoreSheetEntryEvents.Single(x => x.ScoreSheetEntryPenaltyId == leavesPenaltyBox.ScoreSheetEntryPenaltyId && x.EventType == ScoreSheetEntryEventType.PenaltyEnd);
                  scoreSheetEntryEvents.Remove(eventToRemove);
                }
              }
              else
              {
                // away team scored...shorthanded
                results.Add(new ScoreSheetEntryGoalType()
                {
                  ScoreSheetEntryGoalId = Convert.ToInt32(sseEvent.ScoreSheetEntryGoalId),
                  PowerPlayGoal = false,
                  ShortHandedGoal = true,
                  GameWinningGoal = false
                });
              }
            }
            else
            {
              // the away team is on the power play
              if (sseEvent.HomeTeam)
              {
                // home team scored...shorthanded
                results.Add(new ScoreSheetEntryGoalType()
                {
                  ScoreSheetEntryGoalId = Convert.ToInt32(sseEvent.ScoreSheetEntryGoalId),
                  PowerPlayGoal = false,
                  ShortHandedGoal = true,
                  GameWinningGoal = false
                });
              }
              else
              {
                // away team scored...on the power play
                results.Add(new ScoreSheetEntryGoalType()
                {
                  ScoreSheetEntryGoalId = Convert.ToInt32(sseEvent.ScoreSheetEntryGoalId),
                  PowerPlayGoal = true,
                  ShortHandedGoal = false,
                  GameWinningGoal = false
                });

                //release person from home team penalty box (but not if match penalty)
                leavesPenaltyBox = homeTeamPenaltyBox.Where(x => x.MatchPenalty == false).OrderBy(x => x.TimeElapsed).FirstOrDefault();
                if (leavesPenaltyBox != null)
                {
                  homeTeamPenaltyBox.Remove(leavesPenaltyBox);

                  // also remove the penalty end event
                  eventToRemove = scoreSheetEntryEvents.Single(x => x.ScoreSheetEntryPenaltyId == leavesPenaltyBox.ScoreSheetEntryPenaltyId && x.EventType == ScoreSheetEntryEventType.PenaltyEnd);
                  scoreSheetEntryEvents.Remove(eventToRemove);
                }
              }
            }

            // remove event since it has been handled
            eventToRemove = scoreSheetEntryEvents.Single(x => x.ScoreSheetEntryGoalId == sseEvent.ScoreSheetEntryGoalId && x.EventType == sseEvent.EventType);
            scoreSheetEntryEvents.Remove(eventToRemove);
            break;
          default:
            throw new NotImplementedException("The EventType (" + sseEvent.EventType.ToString() + ") is not implemented");
        }
      }

      return results;
    }

    public int? ConvertPlayerNumberIntoPlayer(ICollection<GameRoster> gameRoster, string playerNumber)
    {
      int? playerId = null;

      if (playerNumber != null)
      {
        var gameRosterMatch = gameRoster.Where(x => x.PlayerNumber == playerNumber).FirstOrDefault();
        if (gameRosterMatch == null)
        {
          playerId = 0; // the unknown player
        }
        else
        {
          playerId = gameRosterMatch.PlayerId;
        }
      }

      return playerId;
    }

    public List<ScoreSheetEntryProcessedGoal> UpdateScoreSheetEntryProcessedGoalsWithPPAndSH(List<ScoreSheetEntryProcessedGoal> scoreSheetEntryGoals, List<ScoreSheetEntryProcessedPenalty> scoreSheetEntryPenalties)
    {
      List<ScoreSheetEntryGoalType> scoreSheetEntryGoalTypes = new List<ScoreSheetEntryGoalType>();

      // put all the goals and penalties in order
      List<ScoreSheetEntryEvent> scoreSheetEntryEvents = DeriveScoreSheetEntryEvents(scoreSheetEntryGoals, scoreSheetEntryPenalties);

      int currentGameId = -1;
      int previosGameId = -1;
      List<int> penaltiesProcessing = new List<int>();

      // loop through the events and see if any goals where scored during the penalties
      foreach (var sseEvent in scoreSheetEntryEvents)
      {
        currentGameId = sseEvent.GameId;

        if (currentGameId == previosGameId)
        {
          // this gameid has already been processed; do nothing
        }
        else
        {
          previosGameId = currentGameId;

          var allCurrentGameEvents = scoreSheetEntryEvents.Where(x => x.GameId == currentGameId).OrderBy(x => x.TimeElapsed).ToList();
          var results = ProcessOneGameScoreSheetEntryEvents(allCurrentGameEvents);
          scoreSheetEntryGoalTypes.AddRange(results);
        }
      }


      // loop through goal type results and update score sheet entries
      foreach (var goalTypeResult in scoreSheetEntryGoalTypes)
      {
        var scoreSheetEntry = scoreSheetEntryGoals.Single(x => x.ScoreSheetEntryGoalId == goalTypeResult.ScoreSheetEntryGoalId);
        scoreSheetEntry.PowerPlayGoal = goalTypeResult.PowerPlayGoal;
        scoreSheetEntry.ShortHandedGoal = goalTypeResult.ShortHandedGoal;
      }

      // loop through the penalties and see if any goals where scored during them


      // NOTE THIS WAS COMMENTED OUT
      foreach (var scoreSheetEntryPenalty in scoreSheetEntryPenalties)
      {
        var gameId = scoreSheetEntryPenalty.GameId;
        var penaltyHomeTeam = scoreSheetEntryPenalty.HomeTeam;

        //get start/end time of the penalty
        var start = scoreSheetEntryPenalty.TimeElapsed;
        var end = start.Add(new TimeSpan(0, scoreSheetEntryPenalty.PenaltyMinutes, 0));
        var major = scoreSheetEntryPenalty.PenaltyMinutes == 5 ? true : false;

        //check if there were any goals during the penalty
        var goalsDuringPenalty = scoreSheetEntryGoals.Where(x => x.GameId == gameId && x.TimeElapsed >= start && x.TimeElapsed <= end).OrderBy(x=>x.TimeElapsed).ToList();

        //now see if they were power play or shorthanded
        foreach (var goalDuringPenalty in goalsDuringPenalty)
        {
          // if it was a power play goal (hometeam != penalty hometeam)
          // and it was not a major penalty
          // then the power play is over, no need to continue processing
          if (goalDuringPenalty.HomeTeam != penaltyHomeTeam)
          {
            goalDuringPenalty.PowerPlayGoal = true;

            if (!major)
            {
              // then the power play is over, no need to continue processing
              break; 
            }
          }
          else
          {
            // it was a shorthanded goal
            goalDuringPenalty.ShortHandedGoal = true;
          }
        }
      }

      return scoreSheetEntryGoals;
    }

    public ProcessingResult ProcessScoreSheetEntryPenalties(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var scoreSheetEntryPenalties = _context.ScoreSheetEntryPenalties.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var gameRosters = _context.GameRosters
                                .Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId)
                                .ToList();
        var gameTeams = _context.GameTeams
                        .Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId)
                        .ToList();
        var penalties = _context.Penalties.ToList();

        _logger.Write("ProcessScoreSheetEntryPenalties: scoreSheetEntryPenalties.Count: " + scoreSheetEntryPenalties.Count);

        var scoreSheetEntryPenaltiesProcessed = DeriveScoreSheetEntryProcessedPenalties(scoreSheetEntryPenalties, gameTeams, gameRosters, penalties);

        var modified = _lo30ContextService.SaveOrUpdateScoreSheetEntryProcessedPenalty(scoreSheetEntryPenaltiesProcessed);
        _logger.Write("ProcessScoreSheetEntryPenalties: SaveOrUpdateScoreSheetEntryPenaltyProcessed: " + modified);

        // AUDIT ScoreSheetEntries and ScoreSheetEntriesProcessed should have same ids 
        var inputs = _context.ScoreSheetEntryPenalties.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var outputs = _context.ScoreSheetEntryProcessedPenalties.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();

        if (inputs.Count != outputs.Count)
        {
          result.error = "Error processing ScoreSheetEntryPenalties. The ScoreSheetEntryPenalties count (" + inputs.Count + ") does not match ScoreSheetEntryPenaltiesProcessed count (" + outputs.Count + ")";
        }
        else
        {
          foreach (var input in inputs)
          {
            var output = outputs.Where(x => x.ScoreSheetEntryPenaltyId == input.ScoreSheetEntryPenaltyId).FirstOrDefault();

            if (output == null)
            {
              result.error = "Error processing ScoreSheetEntryPenalties. The ScoreSheetEntryPenaltyId (" + input.ScoreSheetEntryPenaltyId + ") is missing from ScoreSheetEntryPenaltiesProcessed";
            }
          }
        }
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
        //ErrorHandlingService.PrintFullErrorMessage(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult ProcessScoreSheetEntryGoals(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var scoreSheetEntryGoals = _context.ScoreSheetEntryGoals.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var gameTeams = _context.GameTeams.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var gameRosters = _context.GameRosters
                                .Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId)
                                .ToList();

        result.toProcess = scoreSheetEntryGoals.Count;

        var scoreSheetEntryProcessedGoals = DeriveScoreSheetEntryProcessedGoals(scoreSheetEntryGoals, gameTeams, gameRosters);

        result.modified = _lo30ContextService.SaveOrUpdateScoreSheetEntryProcessedGoal(scoreSheetEntryProcessedGoals);

        // AUDIT ScoreSheetEntries and ScoreSheetEntriesProcessed should have same ids 
        var inputs = _context.ScoreSheetEntryGoals.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var outputs = _context.ScoreSheetEntryProcessedGoals.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();

        if (inputs.Count != outputs.Count)
        {
          result.error = "Error processing ScoreSheetEntryGoals. The ScoreSheetEntryGoals count (" + inputs.Count + ") does not match ScoreSheetEntryProcessedGoals count (" + outputs.Count + ")";
        }
        else
        {
          foreach (var input in inputs)
          {
            var output = outputs.Where(x => x.ScoreSheetEntryGoalId == input.ScoreSheetEntryGoalId).FirstOrDefault();

            if (output == null)
            {
              result.error = "Error processing ScoreSheetEntryGoals. The ScoreSheetEntryGoalId (" + input.ScoreSheetEntryGoalId + ") is missing from ScoreSheetEntryProcessedGoals";
            }
          }
        }
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
        //ErrorHandlingService.PrintFullErrorMessage(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult UpdateScoreSheetEntriesWithPPAndSH(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var scoreSheetEntryGoals = _context.ScoreSheetEntryProcessedGoals.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var scoreSheetEntryPenalties = _context.ScoreSheetEntryProcessedPenalties.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();

        result.toProcess = scoreSheetEntryGoals.Count;

        var scoreSheetEntryProcessedGoals = UpdateScoreSheetEntryProcessedGoalsWithPPAndSH(scoreSheetEntryGoals, scoreSheetEntryPenalties);

        result.modified = _lo30ContextService.SaveOrUpdateScoreSheetEntryProcessedGoal(scoreSheetEntryProcessedGoals);

      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
        //ErrorHandlingService.PrintFullErrorMessage(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult ProcessScoreSheetEntrySubs(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var scoreSheetEntrySubs = _context.ScoreSheetEntrySubs.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var players = _context.Players.ToList();
        var games = _context.Games.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var gameTeams = _context.GameTeams.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var teamRosters = _context.TeamRosters.ToList();

        result.toProcess = scoreSheetEntrySubs.Count;

        var scoreSheetEntrySubsProcessed = DeriveScoreSheetEntryProcessedSubs(scoreSheetEntrySubs, games, gameTeams, teamRosters, players);

        result.modified = _lo30ContextService.SaveOrUpdateScoreSheetEntryProcessedSub(scoreSheetEntrySubsProcessed);

        // AUDIT ScoreSheetEntrySubs and ScoreSheetEntrySubsProcessed should have same ids 
        var inputs = _context.ScoreSheetEntrySubs.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var outputs = _context.ScoreSheetEntryProcessedSubs.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();

        if (inputs.Count != outputs.Count)
        {
          result.error = "Error processing ScoreSheetEntrySubs. The ScoreSheetEntrySubs count (" + inputs.Count + ") does not match ScoreSheetEntryProcessedSubs count (" + outputs.Count + ")";
        }
        else
        {
          foreach (var input in inputs)
          {
            var output = outputs.Where(x => x.ScoreSheetEntrySubId == input.ScoreSheetEntrySubId).FirstOrDefault();

            if (output == null)
            {
              result.error = "Error processing ScoreSheetEntrySubs. The ScoreSheetEntrySubId (" + input.ScoreSheetEntrySubId + ") is missing from ScoreSheetEntryProcessedSubs";
            }
          }
        }
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
        //ErrorHandlingService.PrintFullErrorMessage(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult ProcessScoreSheetEntriesIntoGameResults(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        // get list of game entries for these games (use game just in case there was no score sheet entries...0-0 game with no penalty minutes)
        var games = _context.Games.Where(s => s.GameId >= startingGameId && s.GameId <= endingGameId).ToList();

        result.toProcess = games.Count;

        // get a list of periods
        var periods = new int[] { 1, 2, 3, 4 };

        var modifiedCount = 0;

        // loop through each game
        for (var g = 0; g < games.Count; g++)
        {
          var gameId = games[g].GameId;
          int scoreHomeTeamTotal = 0;
          int scoreAwayTeamTotal = 0;
          int penaltyHomeTeamTotal = 0;
          int penaltyAwayTeamTotal = 0;

          // look up the home and away team id
          var homeGameTeam = _lo30ContextService.FindGameTeamByPK2(gameId, homeTeam: true);
          var awayGameTeam = _lo30ContextService.FindGameTeamByPK2(gameId, homeTeam: false);

          #region loop through each period
          for (var p = 0; p < periods.Length; p++)
          {
            var period = periods[p];
            var scoreHomeTeamPeriod = 0;
            var scoreAwayTeamPeriod = 0;

            #region process all score sheet entries for this specific game/period
            var scoreSheetEntries = _context.ScoreSheetEntryProcessedGoals.Where(s => s.GameId == gameId && s.Period == period).ToList();

            for (var s = 0; s < scoreSheetEntries.Count; s++)
            {
              var scoreSheetEntry = scoreSheetEntries[s];

              if (scoreSheetEntry.HomeTeam)
              {
                scoreHomeTeamPeriod++;
                scoreHomeTeamTotal++;
              }
              else
              {
                scoreAwayTeamPeriod++;
                scoreAwayTeamTotal++;
              }
            }
            #endregion

            #region create and save (or update) the home and away teams GameScore by period
            var homeGameScore = new GameScore(sid: homeGameTeam.SeasonId, tid: homeGameTeam.TeamId, gid: gameId, per: period, score: scoreHomeTeamPeriod);
            _lo30ContextService.SaveOrUpdateGameScore(homeGameScore);

            var awayGameScore = new GameScore(sid: awayGameTeam.SeasonId, tid: awayGameTeam.TeamId, gid: gameId, per: period, score: scoreAwayTeamPeriod);
            _lo30ContextService.SaveOrUpdateGameScore(awayGameScore);
            #endregion

            #region process all score sheet entry penalties for this specific game/period
            var scoreSheetEntryPenalties = _context.ScoreSheetEntryPenalties.Where(s => s.GameId == gameId && s.Period == period).ToList();

            for (var s = 0; s < scoreSheetEntryPenalties.Count; s++)
            {
              var scoreSheetEntryPenalty = scoreSheetEntryPenalties[s];

              if (scoreSheetEntryPenalty.HomeTeam)
              {
                penaltyHomeTeamTotal = penaltyHomeTeamTotal + scoreSheetEntryPenalty.PenaltyMinutes;
              }
              else
              {
                penaltyAwayTeamTotal = penaltyAwayTeamTotal + scoreSheetEntryPenalty.PenaltyMinutes;
              }
            }
            #endregion
          }
          #endregion

          #region create and save (or update) the home and away teams GameScore for game
          var finalPeriod = 0;
          var homeFinalGameScore = new GameScore(sid: homeGameTeam.SeasonId, tid: homeGameTeam.TeamId, gid: gameId, per: finalPeriod, score: scoreHomeTeamTotal);
          _lo30ContextService.SaveOrUpdateGameScore(homeFinalGameScore);

          var awayFinalGameScore = new GameScore(sid: awayGameTeam.SeasonId, tid: awayGameTeam.TeamId, gid: gameId, per: finalPeriod, score: scoreAwayTeamTotal);
          _lo30ContextService.SaveOrUpdateGameScore(awayFinalGameScore);
          #endregion

          #region create and save (or update) the home and away teams GameOutcome for game
          // save game results for the game
          string homeResult = "T";
          string awayResult = "T";
          if (scoreHomeTeamTotal > scoreAwayTeamTotal)
          {
            homeResult = "W";
            awayResult = "L";
          }
          else if (scoreHomeTeamTotal < scoreAwayTeamTotal)
          {
            homeResult = "L";
            awayResult = "W";
          }

          var gameRosters = _lo30ContextService.FindGameRostersWithGameId(gameId);

          var gameSubCounts = gameRosters
              .GroupBy(x => new { x.TeamId })
              .Select(grp => new
              {
                TeamId = grp.Key.TeamId,

                Subs = grp.Sum(x => Convert.ToInt32(x.Sub))
              })
              .ToList();

          var homeSubCount = gameSubCounts.Where(x => x.TeamId == homeGameTeam.TeamId).FirstOrDefault().Subs;
          var awaySubCount = gameSubCounts.Where(x => x.TeamId == awayGameTeam.TeamId).FirstOrDefault().Subs;

          var homeGameOutcome = new GameOutcome(
                                        sid: homeGameTeam.SeasonId,
                                        tid: homeGameTeam.TeamId,
                                        gid: gameId,
                                        res: homeResult,
                                        gf: scoreHomeTeamTotal,
                                        ga: scoreAwayTeamTotal,
                                        pim: penaltyHomeTeamTotal,
                                        over: false,
                                        otid: awayGameTeam.TeamId,
                                        subs: homeSubCount
                                        );

          modifiedCount += _lo30ContextService.SaveOrUpdateGameOutcome(homeGameOutcome);

          var awayGameOutcome = new GameOutcome(
                                        sid: awayGameTeam.SeasonId,
                                        tid: awayGameTeam.TeamId,
                                        gid: gameId,
                                        res: awayResult,
                                        gf: scoreAwayTeamTotal,
                                        ga: scoreHomeTeamTotal,
                                        pim: penaltyAwayTeamTotal,
                                        over: false,
                                        otid: homeGameTeam.TeamId,
                                        subs: awaySubCount
                                        );

          modifiedCount += _lo30ContextService.SaveOrUpdateGameOutcome(awayGameOutcome);
          #endregion
        }

        _logger.Write("ProcessScoreSheetEntriesIntoGameResults: savedGameOutcomes:" + modifiedCount);

        result.modified = modifiedCount;
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    // TODO, remove playoffs input and determine it from the gameIds
    public ProcessingResult ProcessGameResultsIntoTeamStandings(int seasonId, bool playoffs, int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      var modified = 0;
      int rank = -1;
      int divisionId = -1;

      try
      {
        // get every team just in case they do not have a game result yet
        var teams = _context.Teams.Where(st => st.SeasonId == seasonId).ToList();

        result.toProcess = teams.Count;

        // loop through each team and calculate their standings data
        for (var t = 0; t < teams.Count; t++)
        {
          var team = teams[t];

          //todo find better way to identify these...field in team table?
          // first 16 teams are the place holders for position night
          if (team.TeamId > 16)
          {
            int games = 0;
            int wins = 0;
            int losses = 0;
            int ties = 0;
            int points = 0;
            int goalsFor = 0;
            int goalsAgainst = 0;
            int penaltyMinutes = 0;
            int subs = 0;

            // get game outcomes for this season team
            var gameOutcomes = _lo30ContextService.FindGameOutcomesWithGameIdsAndTeamId(team.TeamId, startingGameId, endingGameId, errorIfNotFound: false);

            if (gameOutcomes.Count == 0)
            {
              // they haven't played any games yet
              games = 0;
              wins = 0;
              losses = 0;
              ties = 0;
              points = 0;
              goalsAgainst = 0;
              goalsFor = 0;
              penaltyMinutes = 0;
              subs = 0;
            }
            else
            {
              // loop through each game
              //int seasonTypeId=-1; // TODO, make sure the games match seasonTYpe
              for (var g = 0; g < gameOutcomes.Count; g++)
              {
                var gameOutcome = gameOutcomes[g];

                if (gameOutcome.Outcome.ToLower() == "w")
                {
                  wins++;
                }
                else if (gameOutcome.Outcome.ToLower() == "l")
                {
                  losses++;
                }
                else
                {
                  ties++;
                }

                games++;
                points = (wins * 2) + (losses * 0) + (ties * 1);

                goalsFor = goalsFor + gameOutcome.GoalsFor;
                goalsAgainst = goalsAgainst + gameOutcome.GoalsAgainst;
                penaltyMinutes = penaltyMinutes + gameOutcome.PenaltyMinutes;
                subs = subs + gameOutcome.Subs;
              }
            }

            rank = -1;
            divisionId = 1;
            if (playoffs)
            {
              divisionId = Convert.ToInt32(team.DivisionId);
            }

            var teamStanding = new TeamStanding()
            {
              TeamId = team.TeamId,
              Playoffs = playoffs,
              SeasonId = team.SeasonId,
              DivisionId = divisionId,
              Rank = rank,
              Games = games,
              Wins = wins,
              Losses = losses,
              Ties = ties,
              Points = points,
              GoalsFor = goalsFor,
              GoalsAgainst = goalsAgainst,
              PenaltyMinutes = penaltyMinutes,
              Subs = subs
            };

            modified = modified + _lo30ContextService.SaveOrUpdateTeamStanding(teamStanding);
          }
        }

        // now process rank
        var standings = _context.TeamStandings.Where(ts => ts.SeasonId == seasonId)
                            .OrderBy(ts => ts.Playoffs)
                            .ThenBy(ts => ts.DivisionId)
                            .ThenByDescending(ts => ts.Points)
                            .ThenByDescending(ts => ts.Wins)
                            .ThenByDescending(ts => ts.GoalsFor - ts.GoalsAgainst)
                            .ThenByDescending(ts => ts.GoalsFor)
                            .ThenBy(ts => ts.PenaltyMinutes)
                            .ToList();

        divisionId = -1;
        for (var x = 0; x < standings.Count; x++)
        {
          var s = standings[x];

          if (s.DivisionId == divisionId)
          {
            rank = rank + 1;
          }
          else
          {
            rank = 1;
            divisionId = s.DivisionId;
          }

          var teamStanding = new TeamStanding()
          {
            TeamId = s.TeamId,
            Playoffs = s.Playoffs,
            SeasonId = s.SeasonId,
            DivisionId = s.DivisionId,
            Rank = rank,
            Games = s.Games,
            Wins = s.Wins,
            Losses = s.Losses,
            Ties = s.Ties,
            Points = s.Points,
            GoalsFor = s.GoalsFor,
            GoalsAgainst = s.GoalsAgainst,
            PenaltyMinutes = s.PenaltyMinutes,
            Subs = s.Subs
          };

          _lo30ContextService.SaveOrUpdateTeamStanding(teamStanding);
        }

        result.modified = modified;
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult ProcessScoreSheetEntriesIntoPlayerStats(int startingGameId, int endingGameId)
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var gameRosters = _lo30ContextService.FindGameRostersWithGameIds(startingGameId, endingGameId);
        var gameRostersGoalies = _lo30ContextService.FindGameRostersWithGameIdsAndGoalie(startingGameId, endingGameId, goalie: true);
        var gameOutcomes = _lo30ContextService.FindGameOutcomesWithGameIds(startingGameId, endingGameId);
        var ratings = _context.PlayerRatings.ToList();

        var scoreSheetEntryProcessedGoals = _context.ScoreSheetEntryProcessedGoals.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();
        var scoreSheetEntryProcessedPenalties = _context.ScoreSheetEntryProcessedPenalties.Where(x => x.GameId >= startingGameId && x.GameId <= endingGameId).ToList();

        result.toProcess = scoreSheetEntryProcessedGoals.Count + scoreSheetEntryProcessedPenalties.Count;

        var playerGameStats = DerivePlayerGameStats(scoreSheetEntryProcessedGoals, scoreSheetEntryProcessedPenalties, gameRosters);
        var playerTeamStats = DerivePlayerTeamStats(playerGameStats);
        var playerSeasonStats = DerivePlayerSeasonStats(playerTeamStats);

        var goalieGameStats = DeriveGoalieGameStats(gameOutcomes, gameRostersGoalies);
        var goalieTeamStats = DeriveGoalieTeamStats(goalieGameStats);
        var goalieSeasonStats = DeriveGoalieSeasonStats(goalieTeamStats);

        var savedStatsGame = _lo30ContextService.SaveOrUpdatePlayerStatGame(playerGameStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsGame:" + savedStatsGame);

        var savedStatsTeam = _lo30ContextService.SaveOrUpdatePlayerStatTeam(playerTeamStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsTeam:" + savedStatsTeam);

        var savedStatsSeason = _lo30ContextService.SaveOrUpdatePlayerStatSeason(playerSeasonStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsSeason:" + savedStatsSeason);

        var savedStatsGameGoalie = _lo30ContextService.SaveOrUpdateGoalieStatGame(goalieGameStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsGameGoalie:" + savedStatsGameGoalie);

        var savedStatsTeamGoalie = _lo30ContextService.SaveOrUpdateGoalieStatTeam(goalieTeamStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsTeamGoalie:" + savedStatsTeamGoalie);

        var savedStatsSeasonGoalie = _lo30ContextService.SaveOrUpdateGoalieStatSeason(goalieSeasonStats);
        _logger.Write("ProcessScoreSheetEntriesIntoPlayerStats: savedStatsSeasonGoalie:" + savedStatsSeasonGoalie);

        result.modified = (savedStatsGame + savedStatsTeam + savedStatsSeason);
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }

    public ProcessingResult ProcessPlayerStatsIntoWebStats()
    {
      var result = new ProcessingResult();

      DateTime last = DateTime.Now;
      TimeSpan diffFromLast = new TimeSpan();

      try
      {
        var ratings = _context.PlayerRatings.ToList();
        var seasonPlayerStatsForWeb = _context.PlayerStatTeams
                                           .Include("Season")
                                           .Include("Player")
                                           .Include("Team")
                                           .ToList();

        var seasonGoalieStatsForWeb = _context.GoalieStatTeams
                                            .Include("Season")
                                            .Include("Player")
                                            .Include("Team")
                                            .ToList();

        var teamStandingsForWeb = _context.TeamStandings
                                            .Include("Division")
                                            .Include("Team")
                                            .ToList();

        result.toProcess = seasonPlayerStatsForWeb.Count + seasonGoalieStatsForWeb.Count;

        var playerWebStats = DeriveWebPlayerStats(seasonPlayerStatsForWeb, ratings);
        var savedWebPlayerStats = _lo30ContextService.SaveOrUpdateForWebPlayerStat(playerWebStats);
        _logger.Write("ProcessPlayerStatsIntoWebStats: savedWebPlayerStats:" + savedWebPlayerStats);

        var goalieWebStats = DeriveWebGoalieStats(seasonGoalieStatsForWeb);
        var savedGoaliePlayerStats = _lo30ContextService.SaveOrUpdateForWebGoalieStat(goalieWebStats);
        _logger.Write("DeriveWebGoalieStats: savedGoaliePlayerStats:" + savedGoaliePlayerStats);

        var teamStandings = DeriveWebTeamStandings(teamStandingsForWeb);
        var savedTeamStandings = _lo30ContextService.SaveOrUpdateForWebTeamStanding(teamStandings);
        _logger.Write("DeriveWebTeamStandings: savedTeamStandings:" + savedTeamStandings);

        result.modified = (savedWebPlayerStats + savedGoaliePlayerStats);
      }
      catch (Exception ex)
      {
        result.modified = -2;
        result.error = ex.Message;

        _logger.Write(ex);
      }

      diffFromLast = DateTime.Now - last;
      result.time = diffFromLast.ToString();
      return result;
    }
  }
}
