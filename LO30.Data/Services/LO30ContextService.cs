using LO30.Data;
using LO30.Data.Contexts;
using LO30.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace LO30.Data.Services
{
  class ContextTableList
  {
    public string QueryBegin { get; set; }
    public string QueryEnd { get; set; }
    public string TableName { get; set; }
    public string FileName { get; set; }
  }

  public class LO30ContextService
  {
    LO30Context _context;
    private string _folderPath;

    public LO30ContextService(LO30Context ctx)
    {
      _context = ctx;
      _folderPath = @"D:\git\LO30\LO30\App_Data\SqlServer\";
    }

    #region Division Functions (ByPK and PK2, New format)
    #region Find-Division

    public Division FindDivision(int divisionId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Division, bool>> whereClause = x => x.DivisionId == divisionId;

      string errMsgNotFoundFor = " DivisionId:" + divisionId;

      return FindDivision(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public Division FindDivisionByPK2(string divisionLongName, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Division, bool>> whereClause = x => x.DivisionLongName == divisionLongName;

      string errMsgNotFoundFor = " DivisionLongName:" + divisionLongName;

      return FindDivision(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Division FindDivision(Expression<Func<Division, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Division> found;

      if (populateFully)
      {
        found = _context.Divisions.Where(whereClause).ToList();
      }
      else
      {
        found = _context.Divisions.Where(whereClause).ToList();
      }

      return FindBase<Division>(found, "Division", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Divisions
    public int SaveOrUpdateDivision(List<Division> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateDivision(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateDivision(Division toSave)
    {
      Division found;

      // lookup by PK, if it exists
      if (toSave.DivisionId > 0)
      {
        found = FindDivision(toSave.DivisionId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);
      }
      else
      {
        // lookup by PK2
        found = FindDivisionByPK2(toSave.DivisionLongName, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

        // if found, set missing PK
        if (found != null)
        {
          toSave.DivisionId = found.DivisionId;
        }
      }


      if (found == null)
      {
        _context.Divisions.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ForWebGoalieStat Functions (ByPK, New format)
    #region Find-ForWebGoalieStat

    public ForWebGoalieStat FindForWebGoalieStat(int sid, int tid, int pid, bool pfs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ForWebGoalieStat, bool>> whereClause = x => x.SID == sid && x.TID == tid && x.PID == pid && x.PFS == pfs;

      string errMsgNotFoundFor = " SID:" + sid + " TID:" + tid + " PID:" + pid + " PFS:" + pfs;

      return FindForWebGoalieStat(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ForWebGoalieStat FindForWebGoalieStat(Expression<Func<ForWebGoalieStat, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ForWebGoalieStat> found;

      if (populateFully)
      {
        found = _context.ForWebGoalieStats.Where(whereClause).ToList();
      }
      else
      {
        found = _context.ForWebGoalieStats.Where(whereClause).ToList();
      }

      return FindBase<ForWebGoalieStat>(found, "ForWebGoalieStat", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-ForWebGoalieStat
    public int SaveOrUpdateForWebGoalieStat(List<ForWebGoalieStat> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateForWebGoalieStat(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateForWebGoalieStat(ForWebGoalieStat toSave)
    {
      ForWebGoalieStat found = FindForWebGoalieStat(toSave.SID, toSave.TID, toSave.PID, toSave.PFS, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ForWebGoalieStats.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ForWebPlayerStat Functions (ByPK, New format)
    #region Find-ForWebPlayerStat

    public ForWebPlayerStat FindForWebPlayerStat(int sid, int tid, int pid, bool pfs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ForWebPlayerStat, bool>> whereClause = x => x.SID == sid && x.TID == tid && x.PID == pid && x.PFS == pfs;

      string errMsgNotFoundFor = " SID:" + sid + " TID:" + tid + " PID:" + pid + " PFS:" + pfs;

      return FindForWebPlayerStat(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ForWebPlayerStat FindForWebPlayerStat(Expression<Func<ForWebPlayerStat, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ForWebPlayerStat> found;

      if (populateFully)
      {
        found = _context.ForWebPlayerStats.Where(whereClause).ToList();
      }
      else
      {
        found = _context.ForWebPlayerStats.Where(whereClause).ToList();
      }

      return FindBase<ForWebPlayerStat>(found, "ForWebPlayerStat", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-ForWebPlayerStat
    public int SaveOrUpdateForWebPlayerStat(List<ForWebPlayerStat> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateForWebPlayerStat(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateForWebPlayerStat(ForWebPlayerStat toSave)
    {
      ForWebPlayerStat found = FindForWebPlayerStat(toSave.SID, toSave.TID, toSave.PID, toSave.PFS, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ForWebPlayerStats.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ForWebTeamStanding Functions (ByPK, New format)
    #region Find-ForWebTeamStanding

    public ForWebTeamStanding FindForWebTeamStanding(int sid, int tid, bool pfs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ForWebTeamStanding, bool>> whereClause = x => x.SID == sid && x.TID == tid && x.PFS == pfs;

      string errMsgNotFoundFor = " SID:" + sid + " TID:" + tid + " PFS:" + pfs;

      return FindForWebTeamStanding(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ForWebTeamStanding FindForWebTeamStanding(Expression<Func<ForWebTeamStanding, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ForWebTeamStanding> found;

      if (populateFully)
      {
        found = _context.ForWebTeamStandings.Where(whereClause).ToList();
      }
      else
      {
        found = _context.ForWebTeamStandings.Where(whereClause).ToList();
      }

      return FindBase<ForWebTeamStanding>(found, "ForWebTeamStanding", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-ForWebTeamStanding
    public int SaveOrUpdateForWebTeamStanding(List<ForWebTeamStanding> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateForWebTeamStanding(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateForWebTeamStanding(ForWebTeamStanding toSave)
    {
      ForWebTeamStanding found = FindForWebTeamStanding(toSave.SID, toSave.TID, toSave.PFS, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ForWebTeamStandings.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Game Functions (ByPK, New format)
    #region Find-Game
    public Game FindGame(int gameId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Game, bool>> whereClause = x => x.GameId == gameId;

      string errMsgNotFoundFor = " GameId:" + gameId;

      return FindGame(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Game FindGame(Expression<Func<Game, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Game> found;

      if (populateFully)
      {
        found = _context.Games
                            .Include("Season")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.Games.Where(whereClause).ToList();
      }

      return FindBase<Game>(found, "Game", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Game
    public int SaveOrUpdateGame(List<Game> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGame(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGame(Game toSave)
    {
      Game found = FindGame(toSave.GameId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.Games.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GameOutcome Functions (ByPK and addtl finds, New format)
    #region Find-GameOutcome
    public List<GameOutcome> FindGameOutcomesWithGameIdsAndTeamId(int teamId, int startingGameId, int endingGameId, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<GameOutcome, bool>> whereClause = x => x.TeamId == teamId && x.GameId >= startingGameId && x.GameId <= endingGameId;

      string errMsgNotFoundFor = " TeamId:" + teamId +
                                 " GameId between " + startingGameId + " and " + endingGameId
                                ;

      return FindGameOutcomesBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public List<GameOutcome> FindGameOutcomesWithGameIds(int startingGameId, int endingGameId, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<GameOutcome, bool>> whereClause = x => x.GameId >= startingGameId && x.GameId <= endingGameId;

      string errMsgNotFoundFor = " GameId between " + startingGameId + " and " + endingGameId
                                ;

      return FindGameOutcomesBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public GameOutcome FindGameOutcome(int gameId, int teamId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameOutcome, bool>> whereClause = x => x.GameId == gameId && x.TeamId == teamId;

      string errMsgNotFoundFor = " GameId:" + gameId + " TeamId:" + teamId;

      return FindGameOutcome(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GameOutcome FindGameOutcome(Expression<Func<GameOutcome, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GameOutcome> found;

      if (populateFully)
      {
        found = _context.GameOutcomes
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Include("OpponentTeam")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.GameOutcomes.Where(whereClause).ToList();
      }

      return FindBase<GameOutcome>(found, "GameOutcome", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    private List<GameOutcome> FindGameOutcomesBase(Expression<Func<GameOutcome, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool populateFully)
    {
      List<GameOutcome> found;

      if (populateFully)
      {
        found = _context.GameOutcomes
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Include("OpponentTeam")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.GameOutcomes.Where(whereClause)
                                .ToList();
      }

      return FindListBase<GameOutcome>(found, "GameOutcome", errMsgNotFoundFor, errorIfNotFound);
    }
    #endregion

    #region SaveOrUpdate-GameOutcome
    public int SaveOrUpdateGameOutcome(List<GameOutcome> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGameOutcome(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGameOutcome(GameOutcome toSave)
    {
      GameOutcome found = FindGameOutcome(toSave.GameId, toSave.TeamId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.GameOutcomes.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GameRosters Functions (ByPK and PK2 and addtl finds, New format)
    public List<GameRoster> FindGameRostersWithGameId(int gameId, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<GameRoster, bool>> whereClause = x => x.GameId == gameId;

      string errMsgNotFoundFor = " GameId:" + gameId
                                ;

      return FindGameRostersBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public List<GameRoster> FindGameRostersWithGameIds(int startingGameId, int endingGameId, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<GameRoster, bool>> whereClause = x => x.GameId >= startingGameId && x.GameId <= endingGameId;

      string errMsgNotFoundFor = " GameId between " + startingGameId + " and " + endingGameId
                                ;

      return FindGameRostersBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public List<GameRoster> FindGameRostersWithGameIdsAndGoalie(int startingGameId, int endingGameId, bool goalie, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<GameRoster, bool>> whereClause = x => x.GameId >= startingGameId && x.GameId <= endingGameId && x.Goalie == goalie;

      string errMsgNotFoundFor = " GameId between " + startingGameId + " and " + endingGameId + 
                                 " Goalie: " + goalie
                                ;

      return FindGameRostersBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public GameRoster FindGameRosterByPK2(int seasonId, int teamId, int gameId, string playerNumber, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameRoster, bool>> whereClause = x => x.SeasonId == seasonId && x.TeamId == teamId && x.GameId == gameId && x.PlayerNumber == playerNumber;

      string errMsgNotFoundFor = " SeasonId:" + seasonId +
                                  " TeamId:" + teamId +
                                  " GameId:" + gameId +
                                  " PlayerNumber:" + playerNumber
                                  ;

      return FindGameRoster(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public GameRoster FindGameRoster(int gameRosterId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameRoster, bool>> whereClause = x => x.GameRosterId == gameRosterId;

      string errMsgNotFoundFor = " GameRosterId:" + gameRosterId;

      return FindGameRoster(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GameRoster FindGameRoster(Expression<Func<GameRoster, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GameRoster> found;

      if (populateFully)
      {
        found = _context.GameRosters
                        .Include("Season")
                        .Include("Team")
                        .Include("Game")
                        .Include("Player")
                        .Include("SubbingForPlayer")
                        .Where(whereClause).ToList();
      }
      else
      {
        found = _context.GameRosters.Where(whereClause).ToList();
      }

      return FindBase<GameRoster>(found, "GameRoster", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    private List<GameRoster> FindGameRostersBase(Expression<Func<GameRoster, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool populateFully)
    {
      List<GameRoster> found;

      if (populateFully)
      {
        found = _context.GameRosters
                        .Include("Season")
                        .Include("Team")
                        .Include("Game")
                        .Include("Player")
                        .Include("SubbingForPlayer")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.GameRosters.Where(whereClause)
                                .ToList();
      }

      return FindListBase<GameRoster>(found, "GameRoster", errMsgNotFoundFor, errorIfNotFound);
    }
    #region SaveOrUpdate-GameRoster
    public int SaveOrUpdateGameRoster(List<GameRoster> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGameRoster(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGameRoster(GameRoster toSave)
    {
      GameRoster found;

      // lookup by PK, if it exists
      if (toSave.GameRosterId > 0)
      {
        found = FindGameRoster(toSave.GameRosterId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);
      }
      else
      {
        // lookup by PK2
        found = FindGameRosterByPK2(toSave.SeasonId, toSave.TeamId, toSave.GameId, toSave.PlayerNumber, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

        // if found, set missing PK
        if (found != null)
        {
          toSave.GameRosterId = found.GameRosterId;
        }
      }

      if (found == null)
      {
        _context.GameRosters.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GameScore Functions (ByPK, New format)
    #region Find-GameScore
    public GameScore FindGameScore(int gameId, int teamId, int period, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameScore, bool>> whereClause = x => x.GameId == gameId && x.TeamId == teamId && x.Period == period;

      string errMsgNotFoundFor = " GameId:" + gameId + " TeamId:" + teamId + " Period:" + period;

      return FindGameScore(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GameScore FindGameScore(Expression<Func<GameScore, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GameScore> found;

      if (populateFully)
      {
        found = _context.GameScores
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.GameScores.Where(whereClause).ToList();
      }

      return FindBase<GameScore>(found, "GameScore", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-GameScore
    public int SaveOrUpdateGameScore(List<GameScore> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGameScore(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGameScore(GameScore toSave)
    {
      GameScore found = FindGameScore(toSave.GameId, toSave.TeamId, toSave.Period, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.GameScores.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GameTeam Functions (ByPK and PK2, New format)
    #region Find-GameTeam
    public GameTeam FindGameTeamByPK2(int gameId, bool homeTeam, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameTeam, bool>> whereClause = x => x.GameId == gameId && x.HomeTeam == homeTeam;

      string errMsgNotFoundFor = " GameId:" + gameId +
                                " HomeTeam:" + homeTeam
                                ;

      return FindGameTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);

    }

    public GameTeam FindGameTeam(int gameId, int teamId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GameTeam, bool>> whereClause = x => x.GameId == gameId && x.TeamId == teamId;

      string errMsgNotFoundFor = " GameId:" + gameId +
                                " TeamId:" + teamId
                                ;

      return FindGameTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GameTeam FindGameTeam(Expression<Func<GameTeam, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GameTeam> found;

      if (populateFully)
      {
        found = _context.GameTeams
                            .Include("Season")
                            .Include("Team")
                            .Include("Team.Season")
                            .Include("Team.Coach")
                            .Include("Team.Sponsor")
                            .Include("Game")
                            .Include("Game.Season")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.GameTeams.Where(whereClause).ToList();
      }

      return FindBase<GameTeam>(found, "GameTeam", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-GameTeam
    public int SaveOrUpdateGameTeam(List<GameTeam> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGameTeam(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGameTeam(GameTeam toSave)
    {
      GameTeam found;

      // lookup by PK, if it exists
      if (toSave.GameId > 0 && toSave.TeamId > 0)
      {
        found = FindGameTeam(toSave.GameId, toSave.TeamId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);
      }
      else
      {
        // lookup by PK2
        found = FindGameTeamByPK2(toSave.GameId, toSave.HomeTeam, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

        // if found, set missing PK
        if (found != null)
        {
          toSave.GameId = found.GameId;
          toSave.TeamId = found.TeamId;
        }
      }

      if (found == null)
      {
        _context.GameTeams.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GoalieStatGame Functions (ByPK, New format)
    #region Find-GoalieStatGame

    public GoalieStatGame FindGoalieStatGame(int seasonId, int teamId, int gameId, bool playoffs, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GoalieStatGame, bool>> whereClause = x => x.SeasonId == seasonId && x.TeamId == teamId && x.GameId == gameId && x.Playoffs == playoffs && x.PlayerId == playerId;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " TeamId:" + teamId + " GameId:" + gameId + " Playoffs:" + playoffs + " PlayerId:" + playerId;

      return FindGoalieStatGame(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GoalieStatGame FindGoalieStatGame(Expression<Func<GoalieStatGame, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GoalieStatGame> found;

      if (populateFully)
      {
        found = _context.GoalieStatGames
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.GoalieStatGames.Where(whereClause).ToList();
      }

      return FindBase<GoalieStatGame>(found, "GoalieStatGame", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-GoalieStatGame
    public int SaveOrUpdateGoalieStatGame(List<GoalieStatGame> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGoalieStatGame(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGoalieStatGame(GoalieStatGame toSave)
    {
      GoalieStatGame found = FindGoalieStatGame(toSave.SeasonId, toSave.TeamId, toSave.GameId, toSave.Playoffs, toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.GoalieStatGames.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GoalieStatSeason Functions (ByPK, New format)
    #region Find-GoalieStatSeason

    public GoalieStatSeason FindGoalieStatSeason(int seasonId, bool playoffs, int playerId, bool sub, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GoalieStatSeason, bool>> whereClause = x => x.SeasonId == seasonId && x.Playoffs == playoffs && x.PlayerId == playerId && x.Sub == sub;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " Playoffs:" + playoffs + " PlayerId:" + playerId + " Sub:" + sub;

      return FindGoalieStatSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GoalieStatSeason FindGoalieStatSeason(Expression<Func<GoalieStatSeason, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GoalieStatSeason> found;

      if (populateFully)
      {
        found = _context.GoalieStatSeasons
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.GoalieStatSeasons.Where(whereClause).ToList();
      }

      return FindBase<GoalieStatSeason>(found, "GoalieStatSeason", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-GoalieStatSeason
    public int SaveOrUpdateGoalieStatSeason(List<GoalieStatSeason> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGoalieStatSeason(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGoalieStatSeason(GoalieStatSeason toSave)
    {
      GoalieStatSeason found = FindGoalieStatSeason(toSave.SeasonId, toSave.Playoffs, toSave.PlayerId, toSave.Sub, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.GoalieStatSeasons.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region GoalieStatTeam Functions (ByPK, New format)
    #region Find-GoalieStatTeam

    public GoalieStatTeam FindGoalieStatTeam(int seasonId, int teamId, bool playoffs, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<GoalieStatTeam, bool>> whereClause = x => x.SeasonId == seasonId && x.TeamId == teamId && x.Playoffs == playoffs && x.PlayerId == playerId;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " TeamId:" + teamId + " Playoffs:" + playoffs + " PlayerId:" + playerId;

      return FindGoalieStatTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private GoalieStatTeam FindGoalieStatTeam(Expression<Func<GoalieStatTeam, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<GoalieStatTeam> found;

      if (populateFully)
      {
        found = _context.GoalieStatTeams
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.GoalieStatTeams.Where(whereClause).ToList();
      }

      return FindBase<GoalieStatTeam>(found, "GoalieStatTeam", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-GoalieStatTeam
    public int SaveOrUpdateGoalieStatTeam(List<GoalieStatTeam> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateGoalieStatTeam(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateGoalieStatTeam(GoalieStatTeam toSave)
    {
      GoalieStatTeam found = FindGoalieStatTeam(toSave.SeasonId, toSave.TeamId, toSave.Playoffs, toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.GoalieStatTeams.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Penalty Functions (ByPK, New format)
    #region Find-Penalty

    public Penalty FindPenalty(int penaltyId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Penalty, bool>> whereClause = x => x.PenaltyId == penaltyId;

      string errMsgNotFoundFor = " PenaltyId:" + penaltyId;

      return FindPenalty(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Penalty FindPenalty(Expression<Func<Penalty, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Penalty> found;

      if (populateFully)
      {
        found = _context.Penalties.Where(whereClause).ToList();
      }
      else
      {
        found = _context.Penalties.Where(whereClause).ToList();
      }

      return FindBase<Penalty>(found, "Penalty", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Penalty
    public int SaveOrUpdatePenalty(List<Penalty> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePenalty(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePenalty(Penalty toSave)
    {
      Penalty found = FindPenalty(toSave.PenaltyId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.Penalties.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region PlayerDraft Functions (ByPK, New Format)
    #region Find-PlayerDraft
    public PlayerDraft FindPlayerDraft(int seasonId, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerDraft, bool>> whereClause = x => x.SeasonId == seasonId &&
                                                          x.PlayerId == playerId
                                                          ;

      string errMsgNotFoundFor = " SeasonId:" + seasonId +
                                " PlayerId:" + playerId
                                ;

      return FindPlayerDraft(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerDraft FindPlayerDraft(Expression<Func<PlayerDraft, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerDraft> found;

      if (populateFully)
      {
        found = _context.PlayerDrafts
                            .Include("Player")
                            .Include("Season")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.PlayerDrafts.Where(whereClause).ToList();
      }

      return FindBase<PlayerDraft>(found, "PlayerDraft", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerDraft
    public int SaveOrUpdatePlayerDraft(List<PlayerDraft> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerDraft(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerDraft(PlayerDraft toSave)
    {
      PlayerDraft found = FindPlayerDraft(toSave.SeasonId, toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        found = _context.PlayerDrafts.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }

    #endregion
    #endregion

    #region PlayerRating Functions (ByPK, ByYYYYMMDD, New Format)
    #region Find-PlayerRating
    public PlayerRating FindPlayerRatingWithYYYYMMDD(int playerId, string position, int seasonId, int yyyymmdd, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerRating, bool>> whereClause = x => x.SeasonId == seasonId &&
                                                          x.PlayerId == playerId &&
                                                          (x.Position == position || x.Position == "X") &&
                                                          x.StartYYYYMMDD <= yyyymmdd &&
                                                          x.EndYYYYMMDD >= yyyymmdd
                                                          ;

      string errMsgNotFoundFor = " PlayerId:" + playerId +
                                " Position:" + position +
                                " seasonId:" + seasonId +
                                " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                ;

      return FindPlayerRating(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public PlayerRating FindPlayerRating(int seasonId, int playerId, int startYYYYMMDD, int endYYYYMMDD, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerRating, bool>> whereClause = x => x.SeasonId == seasonId &&
                                                          x.PlayerId == playerId &&
                                                          x.StartYYYYMMDD == startYYYYMMDD &&
                                                          x.EndYYYYMMDD == endYYYYMMDD
                                                          ;

      string errMsgNotFoundFor = " SeasonId:" + seasonId +
                                " PlayerId:" + playerId +
                                " StartYYYYMMDD:" + startYYYYMMDD +
                                " EndYYYYMMDD:" + endYYYYMMDD
                                ;

      return FindPlayerRating(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerRating FindPlayerRating(Expression<Func<PlayerRating, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerRating> found;

      if (populateFully)
      {
        found = _context.PlayerRatings
                            .Include("Player")
                            .Include("Season")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.PlayerRatings.Where(whereClause).ToList();
      }

      return FindBase<PlayerRating>(found, "PlayerRating", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerRating
    public int SaveOrUpdatePlayerRating(List<PlayerRating> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerRating(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerRating(PlayerRating toSave)
    {
      PlayerRating found = FindPlayerRating(toSave.SeasonId, toSave.PlayerId, toSave.StartYYYYMMDD, toSave.EndYYYYMMDD, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        found = _context.PlayerRatings.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Player Functions (ByPK, New format)
    #region Find-Player

    public Player FindPlayer(int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Player, bool>> whereClause = x => x.PlayerId == playerId;

      string errMsgNotFoundFor = " PlayerId:" + playerId;

      return FindPlayer(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Player FindPlayer(Expression<Func<Player, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Player> found;

      if (populateFully)
      {
        found = _context.Players.Where(whereClause).ToList();
      }
      else
      {
        found = _context.Players.Where(whereClause).ToList();
      }

      return FindBase<Player>(found, "Player", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Player
    public int SaveOrUpdatePlayer(List<Player> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayer(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayer(Player toSave)
    {
      Player found = FindPlayer(toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.Players.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region PlayerStatGame Functions (ByPK, New format)
    #region Find-PlayerStatGame

    public PlayerStatGame FindPlayerStatGame(int seasonId, int teamId, int gameId, bool playoffs, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerStatGame, bool>> whereClause = x => x.SeasonId == seasonId && x.TeamId == teamId && x.GameId == gameId && x.Playoffs == playoffs && x.PlayerId == playerId;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " TeamId:" + teamId + " GameId:" + gameId + " Playoffs:" + playoffs + " PlayerId:" + playerId;

      return FindPlayerStatGame(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerStatGame FindPlayerStatGame(Expression<Func<PlayerStatGame, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerStatGame> found;

      if (populateFully)
      {
        found = _context.PlayerStatGames
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.PlayerStatGames.Where(whereClause).ToList();
      }

      return FindBase<PlayerStatGame>(found, "PlayerStatGame", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerStatGame
    public int SaveOrUpdatePlayerStatGame(List<PlayerStatGame> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerStatGame(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerStatGame(PlayerStatGame toSave)
    {
      PlayerStatGame found = FindPlayerStatGame(toSave.SeasonId, toSave.TeamId, toSave.GameId, toSave.Playoffs, toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.PlayerStatGames.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region PlayerStatSeason Functions (ByPK, New format)
    #region Find-PlayerStatSeason

    public PlayerStatSeason FindPlayerStatSeason(int seasonId, bool playoffs, int playerId, bool sub, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerStatSeason, bool>> whereClause = x => x.SeasonId == seasonId && x.Playoffs == playoffs && x.PlayerId == playerId && x.Sub == sub;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " Playoffs:" + playoffs + " PlayerId:" + playerId + " Sub:" + sub;

      return FindPlayerStatSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerStatSeason FindPlayerStatSeason(Expression<Func<PlayerStatSeason, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerStatSeason> found;

      if (populateFully)
      {
        found = _context.PlayerStatSeasons
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.PlayerStatSeasons.Where(whereClause).ToList();
      }

      return FindBase<PlayerStatSeason>(found, "PlayerStatSeason", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerStatSeason
    public int SaveOrUpdatePlayerStatSeason(List<PlayerStatSeason> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerStatSeason(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerStatSeason(PlayerStatSeason toSave)
    {
      PlayerStatSeason found = FindPlayerStatSeason(toSave.SeasonId, toSave.Playoffs, toSave.PlayerId, toSave.Sub, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.PlayerStatSeasons.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region PlayerStatTeam Functions (ByPK, New format)
    #region Find-PlayerStatTeam

    public PlayerStatTeam FindPlayerStatTeam(int seasonId, int teamId, bool playoffs, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerStatTeam, bool>> whereClause = x => x.SeasonId == seasonId && x.TeamId == teamId && x.Playoffs == playoffs && x.PlayerId == playerId;

      string errMsgNotFoundFor = " SeasonId:" + seasonId + " TeamId:" + teamId + " Playoffs:" + playoffs + " PlayerId:" + playerId;

      return FindPlayerStatTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerStatTeam FindPlayerStatTeam(Expression<Func<PlayerStatTeam, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerStatTeam> found;

      if (populateFully)
      {
        found = _context.PlayerStatTeams
                          .Include("Season")
                          .Include("Team")
                          .Include("Game")
                          .Include("Player")
                          .Where(whereClause).ToList();
      }
      else
      {
        found = _context.PlayerStatTeams.Where(whereClause).ToList();
      }

      return FindBase<PlayerStatTeam>(found, "PlayerStatTeam", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerStatTeam
    public int SaveOrUpdatePlayerStatTeam(List<PlayerStatTeam> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerStatTeam(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerStatTeam(PlayerStatTeam toSave)
    {
      PlayerStatTeam found = FindPlayerStatTeam(toSave.SeasonId, toSave.TeamId, toSave.Playoffs, toSave.PlayerId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.PlayerStatTeams.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region PlayerStatus Functions (ByPK, New format)
    #region Find-PlayerStatus

    public PlayerStatus FindPlayerStatus(int playerStatusId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<PlayerStatus, bool>> whereClause = x => x.PlayerStatusId == playerStatusId;

      string errMsgNotFoundFor = " PlayerStatusId:" + playerStatusId;

      return FindPlayerStatus(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private PlayerStatus FindPlayerStatus(Expression<Func<PlayerStatus, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<PlayerStatus> found;

      if (populateFully)
      {
        found = _context.PlayerStatuses.Where(whereClause).ToList();
      }
      else
      {
        found = _context.PlayerStatuses.Where(whereClause).ToList();
      }

      return FindBase<PlayerStatus>(found, "PlayerStatus", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-PlayerStatus
    public int SaveOrUpdatePlayerStatus(List<PlayerStatus> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdatePlayerStatus(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdatePlayerStatus(PlayerStatus toSave)
    {
      PlayerStatus found = FindPlayerStatus(toSave.PlayerStatusId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.PlayerStatuses.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntryGoal Functions (ByPK, New Format)
    #region Find-ScoreSheetEntryGoal

    public ScoreSheetEntryGoal FindScoreSheetEntryGoal(int scoreSheetEntryGoalId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntryGoal, bool>> whereClause = x => x.ScoreSheetEntryGoalId == scoreSheetEntryGoalId;

      string errMsgNotFoundFor = " ScoreSheetEntryGoalId:" + scoreSheetEntryGoalId
                                ;

      return FindScoreSheetEntryGoal(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntryGoal FindScoreSheetEntryGoal(Expression<Func<ScoreSheetEntryGoal, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntryGoal> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntryGoals
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntryGoals.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntryGoal>(found, "ScoreSheetEntryGoal", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntryGoal
    public int SaveOrUpdateScoreSheetEntryGoal(List<ScoreSheetEntryGoal> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntryGoal(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntryGoal(ScoreSheetEntryGoal toSave)
    {
      ScoreSheetEntryGoal found = FindScoreSheetEntryGoal(toSave.ScoreSheetEntryGoalId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntryGoals.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntryPenalty Functions (ByPK, New Format)
    #region Find-ScoreSheetEntryPenalty

    public ScoreSheetEntryPenalty FindScoreSheetEntryPenalty(int scoreSheetEntryPenaltyId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntryPenalty, bool>> whereClause = x => x.ScoreSheetEntryPenaltyId == scoreSheetEntryPenaltyId;

      string errMsgNotFoundFor = " ScoreSheetEntryPenaltyId:" + scoreSheetEntryPenaltyId
                                ;

      return FindScoreSheetEntryPenalty(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntryPenalty FindScoreSheetEntryPenalty(Expression<Func<ScoreSheetEntryPenalty, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntryPenalty> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntryPenalties
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntryPenalties.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntryPenalty>(found, "ScoreSheetEntryPenalty", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntryPenalty
    public int SaveOrUpdateScoreSheetEntryPenalty(List<ScoreSheetEntryPenalty> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntryPenalty(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntryPenalty(ScoreSheetEntryPenalty toSave)
    {
      ScoreSheetEntryPenalty found = FindScoreSheetEntryPenalty(toSave.ScoreSheetEntryPenaltyId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntryPenalties.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntryProcessedGoal Functions (ByPK, New Format)
    #region Find-ScoreSheetEntryProcessedGoal

    public ScoreSheetEntryProcessedGoal FindScoreSheetEntryProcessedGoal(int scoreSheetEntryGoalId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntryProcessedGoal, bool>> whereClause = x => x.ScoreSheetEntryGoalId == scoreSheetEntryGoalId;

      string errMsgNotFoundFor = " ScoreSheetEntryGoalId:" + scoreSheetEntryGoalId
                                ;

      return FindScoreSheetEntryProcessedGoal(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntryProcessedGoal FindScoreSheetEntryProcessedGoal(Expression<Func<ScoreSheetEntryProcessedGoal, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntryProcessedGoal> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntryProcessedGoals
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Include("GoalPlayer")
                            .Include("Assist1Player")
                            .Include("Assist2Player")
                            .Include("Assist3Player")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntryProcessedGoals.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntryProcessedGoal>(found, "ScoreSheetEntryProcessedGoal", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntryProcessedGoal
    public int SaveOrUpdateScoreSheetEntryProcessedGoal(List<ScoreSheetEntryProcessedGoal> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntryProcessedGoal(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntryProcessedGoal(ScoreSheetEntryProcessedGoal toSave)
    {
      ScoreSheetEntryProcessedGoal found = FindScoreSheetEntryProcessedGoal(toSave.ScoreSheetEntryGoalId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntryProcessedGoals.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntryProcessedPenalty Functions (ByPK, New Format)
    #region Find-ScoreSheetEntryProcessedPenalty

    public ScoreSheetEntryProcessedPenalty FindScoreSheetEntryProcessedPenalty(int scoreSheetEntryPenaltyId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntryProcessedPenalty, bool>> whereClause = x => x.ScoreSheetEntryPenaltyId == scoreSheetEntryPenaltyId;

      string errMsgNotFoundFor = " ScoreSheetEntryPenaltyId:" + scoreSheetEntryPenaltyId
                                ;

      return FindScoreSheetEntryProcessedPenalty(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntryProcessedPenalty FindScoreSheetEntryProcessedPenalty(Expression<Func<ScoreSheetEntryProcessedPenalty, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntryProcessedPenalty> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntryProcessedPenalties
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Include("Player")
                            .Include("Penalty")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntryProcessedPenalties.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntryProcessedPenalty>(found, "ScoreSheetEntryProcessedPenalty", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntryProcessedPenalty
    public int SaveOrUpdateScoreSheetEntryProcessedPenalty(List<ScoreSheetEntryProcessedPenalty> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntryProcessedPenalty(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntryProcessedPenalty(ScoreSheetEntryProcessedPenalty toSave)
    {
      ScoreSheetEntryProcessedPenalty found = FindScoreSheetEntryProcessedPenalty(toSave.ScoreSheetEntryPenaltyId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntryProcessedPenalties.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntrySub Functions (ByPK, New Format)
    #region Find-ScoreSheetEntrySub

    public ScoreSheetEntrySub FindScoreSheetEntrySub(int scoreSheetEntrySubId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntrySub, bool>> whereClause = x => x.ScoreSheetEntrySubId == scoreSheetEntrySubId;

      string errMsgNotFoundFor = " ScoreSheetEntrySubId:" + scoreSheetEntrySubId
                                ;

      return FindScoreSheetEntrySub(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntrySub FindScoreSheetEntrySub(Expression<Func<ScoreSheetEntrySub, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntrySub> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntrySubs
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntrySubs.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntrySub>(found, "ScoreSheetEntrySub", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntrySub
    public int SaveOrUpdateScoreSheetEntrySub(List<ScoreSheetEntrySub> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntrySub(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntrySub(ScoreSheetEntrySub toSave)
    {
      ScoreSheetEntrySub found = FindScoreSheetEntrySub(toSave.ScoreSheetEntrySubId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntrySubs.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region ScoreSheetEntryProcessedSub Functions (ByPK, New Format)
    #region Find-ScoreSheetEntryProcessedSub
    public ScoreSheetEntryProcessedSub FindScoreSheetEntryProcessedSub(int scoreSheetEntrySubId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<ScoreSheetEntryProcessedSub, bool>> whereClause = x => x.ScoreSheetEntrySubId == scoreSheetEntrySubId;

      string errMsgNotFoundFor = " ScoreSheetEntrySubId:" + scoreSheetEntrySubId
                                ;

      return FindScoreSheetEntryProcessedSub(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private ScoreSheetEntryProcessedSub FindScoreSheetEntryProcessedSub(Expression<Func<ScoreSheetEntryProcessedSub, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<ScoreSheetEntryProcessedSub> found;

      if (populateFully)
      {
        found = _context.ScoreSheetEntryProcessedSubs
                            .Include("Season")
                            .Include("Team")
                            .Include("Game")
                            .Include("SubPlayer")
                            .Include("SubbingForPlayer")

                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.ScoreSheetEntryProcessedSubs.Where(whereClause).ToList();
      }

      return FindBase<ScoreSheetEntryProcessedSub>(found, "ScoreSheetEntryProcessedSub", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-ScoreSheetEntryProcessedSub
    public int SaveOrUpdateScoreSheetEntryProcessedSub(List<ScoreSheetEntryProcessedSub> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateScoreSheetEntryProcessedSub(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateScoreSheetEntryProcessedSub(ScoreSheetEntryProcessedSub toSave)
    {
      ScoreSheetEntryProcessedSub found = FindScoreSheetEntryProcessedSub(toSave.ScoreSheetEntrySubId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.ScoreSheetEntryProcessedSubs.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Season Functions (ByPK, ByCurrentSeason, ByYYYYMMDD, New format)
    #region Find-Season

    public Season FindSeasonWithYYYYMMDD(int yyyymmdd, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Season, bool>> whereClause = x => x.StartYYYYMMDD >= yyyymmdd && x.EndYYYYMMDD <= yyyymmdd;

      string errMsgNotFoundFor = " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd;

      return FindSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public Season FindSeasonWithIsCurrentSeason(bool isCurrentSeason, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Season, bool>> whereClause = x => x.IsCurrentSeason == isCurrentSeason;

      string errMsgNotFoundFor = " IsCurrentSeason:" + isCurrentSeason;

      return FindSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public Season FindSeason(int seasonId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Season, bool>> whereClause = x => x.SeasonId == seasonId;

      string errMsgNotFoundFor = " SeasonId:" + seasonId;

      return FindSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Season FindSeason(Expression<Func<Season, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Season> found;

      if (populateFully)
      {
        found = _context.Seasons.Where(whereClause).ToList();
      }
      else
      {
        found = _context.Seasons.Where(whereClause).ToList();
      }

      return FindBase<Season>(found, "Season", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Season
    public int SaveOrUpdateSeason(List<Season> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateSeason(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateSeason(Season toSave)
    {
      Season found = FindSeason(toSave.SeasonId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.Seasons.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Setting Functions (ByPK, New format)
    #region Find-Setting

    public Setting FindSetting(int settingId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Setting, bool>> whereClause = x => x.SettingId == settingId;

      string errMsgNotFoundFor = " SettingId:" + settingId;

      return FindSetting(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Setting FindSetting(Expression<Func<Setting, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Setting> found;

      if (populateFully)
      {
        found = _context.Settings.Where(whereClause).ToList();
      }
      else
      {
        found = _context.Settings.Where(whereClause).ToList();
      }

      return FindBase<Setting>(found, "Setting", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }
    #endregion

    #region SaveOrUpdate-Setting
    public int SaveOrUpdateSetting(List<Setting> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateSetting(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateSetting(Setting toSave)
    {
      Setting found = FindSetting(toSave.SettingId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.Settings.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region TeamRoster Functions (ByPK, addtl finds, New Format)
    #region Find-TeamRoster(s) (addtl finds)
    public List<TeamRoster> FindTeamRostersWithYYYYMMDD(int teamId, int yyyymmdd, bool errorIfNotFound = true, bool populateFully = false)
    {
      Expression<Func<TeamRoster, bool>> whereClause = x => x.TeamId == teamId &&
                                                            x.StartYYYYMMDD <= yyyymmdd &&
                                                            x.EndYYYYMMDD >= yyyymmdd
                                                            ;

      string errMsgNotFoundFor = " TeamId:" + teamId +
                                  " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                ;

      return FindTeamRostersBase(whereClause, errMsgNotFoundFor, errorIfNotFound, populateFully);
    }

    public TeamRoster FindTeamRosterWithYYYYMMDD(int teamId, int playerId, int yyyymmdd, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<TeamRoster, bool>> whereClause = x => x.TeamId == teamId &&
                                                            x.PlayerId == playerId &&
                                                            x.StartYYYYMMDD <= yyyymmdd &&
                                                            x.EndYYYYMMDD >= yyyymmdd
                                                            ;

      string errMsgNotFoundFor = " TeamId:" + teamId +
                                  " PlayerId:" + playerId +
                                  " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                ;

      return FindTeamRosterBase(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public TeamRoster FindTeamRosterWithYYYYMMDD(int playerId, int yyyymmdd, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<TeamRoster, bool>> whereClause = x => x.PlayerId == playerId &&
                                                            x.StartYYYYMMDD <= yyyymmdd &&
                                                            x.EndYYYYMMDD >= yyyymmdd
                                                            ;

      string errMsgNotFoundFor = " PlayerId:" + playerId +
                                  " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                ;

      return FindTeamRosterBase(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    public TeamRoster FindTeamRoster(int teamId, int playerId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<TeamRoster, bool>> whereClause = x => x.TeamId == teamId &&
                                                            x.PlayerId == playerId
                                                          ;

      string errMsgNotFoundFor = " TeamId:" + teamId +
                                " PlayerId:" + playerId
                                ;

      return FindTeamRosterBase(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private TeamRoster FindTeamRosterBase(Expression<Func<TeamRoster, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<TeamRoster> found;

      if (populateFully)
      {
        found = _context.TeamRosters
                            .Include("Player")
                            .Include("Season")
                            .Include("Team")
                            .Include("Team.Season")
                            .Include("Team.Coach")
                            .Include("Team.Sponsor")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.TeamRosters.Where(whereClause)
                                .ToList();
      }

      return FindBase<TeamRoster>(found, "TeamRoster", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    private List<TeamRoster> FindTeamRostersBase(Expression<Func<TeamRoster, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool populateFully)
    {
      List<TeamRoster> found;

      if (populateFully)
      {
        found = _context.TeamRosters
                            .Include("Player")
                            .Include("Season")
                            .Include("Team")
                            .Include("Team.Season")
                            .Include("Team.Coach")
                            .Include("Team.Sponsor")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.TeamRosters.Where(whereClause)
                                .ToList();
      }

      return FindListBase<TeamRoster>(found, "TeamRoster", errMsgNotFoundFor, errorIfNotFound);
    }
    #endregion
    #region SaveOrUpdate-TeamRoster
    public int SaveOrUpdateTeamRoster(List<TeamRoster> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateTeamRoster(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateTeamRoster(TeamRoster toSave)
    {
      var errorIfNotFound = false;
      var errorIfMoreThanOneFound = true;
      TeamRoster found = FindTeamRoster(toSave.TeamId, toSave.PlayerId, errorIfNotFound, errorIfMoreThanOneFound);

      if (found == null)
      {
        _context.TeamRosters.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Teams (ByPK, New Format)
    #region Find-Team
    public Team FindTeam(int teamId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<Team, bool>> whereClause = x => x.TeamId == teamId;

      string errMsgNotFoundFor = " TeamId:" + teamId;

      return FindTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private Team FindTeam(Expression<Func<Team, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<Team> found;

      if (populateFully)
      {
        found = _context.Teams
                      .Include("Coach")
                      .Include("Sponsor")
                      .Where(whereClause)
                      .ToList();
      }
      else
      {
        found = _context.Teams.Where(whereClause).ToList();
      }

      return FindBase<Team>(found, "Team", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-Team
    public int SaveOrUpdateTeam(List<Team> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateTeam(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateTeam(Team toSave)
    {
      var errorIfNotFound = false;
      var errorIfMoreThanOneFound = true;
      Team found = FindTeam(toSave.TeamId, errorIfNotFound, errorIfMoreThanOneFound);

      if (found == null)
      {
        _context.Teams.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region TeamStanding Functions (ByPK, New format)
    #region Find-TeamStanding
    public TeamStanding FindTeamStanding(int teamId, bool playoffs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
    {
      Expression<Func<TeamStanding, bool>> whereClause = x => x.TeamId == teamId && x.Playoffs == playoffs;

      string errMsgNotFoundFor = " TeamId:" + teamId +
                                " Playoffs:" + playoffs
                                ;

      return FindTeamStanding(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
    }

    private TeamStanding FindTeamStanding(Expression<Func<TeamStanding, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
    {
      List<TeamStanding> found;

      if (populateFully)
      {
        found = _context.TeamStandings
                            .Include("Season")
                            .Include("Team")
                            .Include("Team.Season")
                            .Include("Team.Coach")
                            .Include("Team.Sponsor")
                            .Where(whereClause)
                            .ToList();
      }
      else
      {
        found = _context.TeamStandings.Where(whereClause).ToList();
      }

      return FindBase<TeamStanding>(found, "TeamStanding", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
    }

    #endregion

    #region SaveOrUpdate-TeamStanding
    public int SaveOrUpdateTeamStanding(List<TeamStanding> listToSave)
    {
      var saved = 0;
      foreach (var toSave in listToSave)
      {
        var results = SaveOrUpdateTeamStanding(toSave);
        saved = saved + results;
      }

      return saved;
    }

    public int SaveOrUpdateTeamStanding(TeamStanding toSave)
    {
      TeamStanding found = FindTeamStanding(toSave.TeamId, toSave.Playoffs, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      if (found == null)
      {
        _context.TeamStandings.Add(toSave);
      }
      else
      {
        var entry = _context.Entry(found);
        entry.OriginalValues.SetValues(found);
        entry.CurrentValues.SetValues(toSave);
      }

      return ContextSaveChanges();
    }
    #endregion
    #endregion

    #region Find Base
    private List<T> FindListBase<T>(List<T> found, string errMsgType, string errMsgNotFoundFor, bool errorIfNotFound)
    {
      if (errorIfNotFound == true && found.Count < 1)
      {
        throw new ArgumentNullException("found", "Could not find " + errMsgType + " for" + errMsgNotFoundFor);
      }

      return found;
    }

    private T FindBase<T>(List<T> found, string errMsgType, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound)
    {
      if (errorIfNotFound == true && found.Count < 1)
      {
        throw new ArgumentNullException("found", "Could not find " + errMsgType + " for" + errMsgNotFoundFor);
      }

      if (errorIfMoreThanOneFound == true && found.Count > 1)
      {
        throw new ArgumentNullException("found", "More than 1 " + errMsgType + " was found for" + errMsgNotFoundFor);
      }

      if (found.Count >= 1)
      {
        return found[0];
      }
      else
      {
        return default(T);
      }
    }

    #endregion

    public int ContextSaveChanges()
    {
      try
      {
        return _context.SaveChanges();
      }
      catch (DbEntityValidationException e)
      {
        foreach (var eve in e.EntityValidationErrors)
        {
          Debug.Print("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
          foreach (var ve in eve.ValidationErrors)
          {
            Debug.Print("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
          }
        }
        throw;
      }
      catch (Exception ex)
      {
        Debug.Print(ex.Message);
        var innerEx = ex.InnerException;

        while (innerEx != null)
        {
          Debug.Print("With inner exception of:");
          Debug.Print(innerEx.Message);

          innerEx = innerEx.InnerException;
        }

        Debug.Print(ex.StackTrace);

        throw ex;
      }
    }

  }
}