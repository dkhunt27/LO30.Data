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
        _folderPath = @"C:\git\LO30\LO30\App_Data\SqlServer\";
      }

      #region Division Functions
      #region Find-Division (addtl finds) NEW FORMAT

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
      
      #region SaveOrUpdate-Divisions (multi lookups)
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

      #region ForWebGoalieStat Functions TODO
      //#region Find-ForWebGoalieStat
      //public ForWebGoalieStat FindForWebGoalieStat(int pid, int stidpf, bool pfs)
      //{
      //  return FindForWebGoalieStat(errorIfNotFound: true, errorIfMoreThanOneFound: true, pid: pid, stidpf: stidpf, pfs: pfs);
      //}

      //public ForWebGoalieStat FindForWebGoalieStat(bool errorIfNotFound, bool errorIfMoreThanOneFound, int pid, int stidpf, bool pfs)
      //{
      //  var found = _context.ForWebGoalieStats.Where(x => x.PID == pid && x.STID == stidpf && x.PFS == pfs).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ForWebGoalieStat for" +
      //                                            " PID:" + pid +
      //                                            " STID:" + stidpf +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ForWebGoalieStat was found for" +
      //                                            " PID:" + pid +
      //                                            " STID:" + stidpf +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ForWebGoalieStat
      //public int SaveOrUpdateForWebGoalieStat(List<ForWebGoalieStat> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateForWebGoalieStat(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateForWebGoalieStat(ForWebGoalieStat toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ForWebGoalieStat found = FindForWebGoalieStat(errorIfNotFound, errorIfMoreThanOneFound, toSave.PID, toSave.STID, toSave.PFS);

      //  if (found == null)
      //  {
      //    _context.ForWebGoalieStats.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region ForWebPlayerStat Functions TODO
      //#region Find-ForWebPlayerStat
      //public ForWebPlayerStat FindForWebPlayerStat(int pid, int stidpf, bool pfs)
      //{
      //  return FindForWebPlayerStat(errorIfNotFound: true, errorIfMoreThanOneFound: true, pid: pid, stidpf: stidpf, pfs: pfs);
      //}

      //public ForWebPlayerStat FindForWebPlayerStat(bool errorIfNotFound, bool errorIfMoreThanOneFound, int pid, int stidpf, bool pfs)
      //{
      //  var found = _context.ForWebPlayerStats.Where(x => x.PID == pid && x.STID == stidpf && x.PFS == pfs).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ForWebPlayerStat for" +
      //                                            " PID:" + pid +
      //                                            " STID:" + stidpf +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ForWebPlayerStat was found for" +
      //                                            " PID:" + pid +
      //                                            " STID:" + stidpf +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ForWebPlayerStat
      //public int SaveOrUpdateForWebPlayerStat(List<ForWebPlayerStat> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateForWebPlayerStat(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateForWebPlayerStat(ForWebPlayerStat toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ForWebPlayerStat found = FindForWebPlayerStat(errorIfNotFound, errorIfMoreThanOneFound, toSave.PID, toSave.STID, toSave.PFS);

      //  if (found == null)
      //  {
      //    _context.ForWebPlayerStats.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region ForWebTeamStanding Functions TODO
      //#region Find-ForWebTeamStanding
      //public ForWebTeamStanding FindForWebTeamStanding(int stid, bool pfs)
      //{
      //  return FindForWebTeamStanding(errorIfNotFound: true, errorIfMoreThanOneFound: true, stid: stid, pfs: pfs);
      //}

      //public ForWebTeamStanding FindForWebTeamStanding(bool errorIfNotFound, bool errorIfMoreThanOneFound, int stid, bool pfs)
      //{
      //  var found = _context.ForWebTeamStandings.Where(x => x.STID == stid && x.PFS == pfs).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ForWebTeamStanding for" +
      //                                            " STID:" + stid +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ForWebTeamStanding was found for" +
      //                                            " STID:" + stid +
      //                                            " PFS:" + pfs
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ForWebTeamStanding
      //public int SaveOrUpdateForWebTeamStanding(List<ForWebTeamStanding> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateForWebTeamStanding(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateForWebTeamStanding(ForWebTeamStanding toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ForWebTeamStanding found = FindForWebTeamStanding(errorIfNotFound, errorIfMoreThanOneFound, toSave.STID, toSave.PFS);

      //  if (found == null)
      //  {
      //    _context.ForWebTeamStandings.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region Game Functions (New format)
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

      #region GameOutcome Functions TODO
      //#region Find-GameOutcome (addtl finds)
      //public List<GameOutcome> FindGameOutcomesWithGameIdsAndTeamId(int startingGameId, int endingGameId, int seasonTeamId)
      //{
      //  return FindGameOutcomesWithGameIdsAndTeamId(errorIfNotFound: true, startingGameId: startingGameId, endingGameId: endingGameId, seasonTeamId: seasonTeamId);
      //}

      //public List<GameOutcome> FindGameOutcomesWithGameIdsAndTeamId(bool errorIfNotFound, int startingGameId, int endingGameId, int seasonTeamId)
      //{
      //  var found = _context.GameOutcomes.Where(x => x.GameTeam.GameId >= startingGameId && x.GameTeam.GameId <= endingGameId && x.GameTeam.SeasonTeamId == seasonTeamId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameOutcome for" +
      //                                            " GameTeam.GameId (starting):" + startingGameId +
      //                                            " GameTeam.GameId (ending):" + endingGameId +
      //                                            " GameTeam.SeasonTeamId:" + seasonTeamId
      //                                    );
      //  }

      //  return found;
      //}

      //public List<GameOutcome> FindGameOutcomesWithGameIds(int startingGameId, int endingGameId)
      //{
      //  return FindGameOutcomesWithGameIds(errorIfNotFound: true, startingGameId: startingGameId, endingGameId: endingGameId);
      //}

      //public List<GameOutcome> FindGameOutcomesWithGameIds(bool errorIfNotFound, int startingGameId, int endingGameId)
      //{
      //  var found = _context.GameOutcomes.Where(x => x.GameTeam.GameId >= startingGameId && x.GameTeam.GameId <= endingGameId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameOutcome for" +
      //                                            " GameTeam.GameId (starting):" + startingGameId +
      //                                            " GameTeam.GameId (ending):" + endingGameId
      //                                    );
      //  }

      //  return found;
      //}

      //public GameOutcome FindGameOutcome(int gameTeamId)
      //{
      //  return FindGameOutcome(errorIfNotFound: true, errorIfMoreThanOneFound: true, gameTeamId: gameTeamId);
      //}

      //public GameOutcome FindGameOutcome(bool errorIfNotFound, bool errorIfMoreThanOneFound, int gameTeamId)
      //{
      //  var found = _context.GameOutcomes.Where(x => x.GameTeamId == gameTeamId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameOutcome for" +
      //                                            " GameTeamId:" + gameTeamId
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 GameOutcome was not found for" +
      //                                            " GameTeamId:" + gameTeamId
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-GameOutcome
      //public int SaveOrUpdateGameOutcome(List<GameOutcome> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateGameOutcome(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateGameOutcome(GameOutcome toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  GameOutcome found = FindGameOutcome(errorIfNotFound, errorIfMoreThanOneFound, toSave.GameTeamId);

      //  if (found == null)
      //  {
      //    _context.GameOutcomes.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region GameRosters Functions (ByPK and PK2, New format)
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

      //#region Find-GameRosters (addtl finds)
      //public List<GameRoster> FindGameRostersWithGameIdsAndGoalie(int startingGameId, int endingGameId, bool goalie)
      //{
      //  return FindGameRostersWithGameIdsAndGoalie(errorIfNotFound: true, startingGameId: startingGameId, endingGameId: endingGameId, goalie: goalie);
      //}

      //public List<GameRoster> FindGameRostersWithGameIdsAndGoalie(bool errorIfNotFound, int startingGameId, int endingGameId, bool goalie)
      //{
      //  var found = _context.GameRosters
      //                    .Include("GameTeam")
      //                    .Include("GameTeam.Game")
      //                    .Include("GameTeam.Game.Season")
      //                    .Include("GameTeam.SeasonTeam")
      //                    .Include("GameTeam.SeasonTeam.Season")
      //                    .Include("GameTeam.SeasonTeam.Team")
      //                    .Include("GameTeam.SeasonTeam.Team.Coach")
      //                    .Include("GameTeam.SeasonTeam.Team.Sponsor")

      //                    .Include("Player")
      //                    .Include("SubbingForPlayer")
      //                    .Where(x => x.GameTeam.GameId >= startingGameId && x.GameTeam.GameId <= endingGameId && x.Goalie == goalie).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameRosters for" +
      //                                            " GameTeam.GameId (starting):" + startingGameId +
      //                                            " GameTeam.GameId (ending):" + endingGameId +
      //                                            " Goalie:" + goalie
      //                                    );
      //  }

      //  return found;
      //}

      //public List<GameRoster> FindGameRostersWithGameIds(int startingGameId, int endingGameId)
      //{
      //  return FindGameRostersWithGameIds(errorIfNotFound: true, startingGameId: startingGameId, endingGameId: endingGameId);
      //}

      //public List<GameRoster> FindGameRostersWithGameIds(bool errorIfNotFound, int startingGameId, int endingGameId)
      //{
      //  var found = _context.GameRosters
      //                    .Include("GameTeam")
      //                    .Include("GameTeam.Game")
      //                    .Include("GameTeam.Game.Season")
      //                    .Include("GameTeam.SeasonTeam")
      //                    .Include("GameTeam.SeasonTeam.Season")
      //                    .Include("GameTeam.SeasonTeam.Team")
      //                    .Include("GameTeam.SeasonTeam.Team.Coach")
      //                    .Include("GameTeam.SeasonTeam.Team.Sponsor")

      //                    .Include("Player")
      //                    .Include("SubbingForPlayer")
      //                    .Where(x => x.GameTeam.GameId >= startingGameId && x.GameTeam.GameId <= endingGameId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameRosters for" +
      //                                            " GameTeam.GameId (starting):" + startingGameId +
      //                                            " GameTeam.GameId (ending):" + endingGameId
      //                                    );
      //  }

      //  return found;
      //}

      //public List<GameRoster> FindGameRostersWithGameId(int gameId)
      //{
      //  return FindGameRostersWithGameId(errorIfNotFound: true, gameId: gameId);
      //}

      //public List<GameRoster> FindGameRostersWithGameId(bool errorIfNotFound, int gameId)
      //{
      //  var found = _context.GameRosters
      //                    .Include("GameTeam")
      //                    .Include("GameTeam.Game")
      //                    .Include("GameTeam.Game.Season")
      //                    .Include("GameTeam.SeasonTeam")
      //                    .Include("GameTeam.SeasonTeam.Season")
      //                    .Include("GameTeam.SeasonTeam.Team")
      //                    .Include("GameTeam.SeasonTeam.Team.Coach")
      //                    .Include("GameTeam.SeasonTeam.Team.Sponsor")

      //                    .Include("Player")
      //                    .Include("SubbingForPlayer")
      //                    .Where(x => x.GameTeam.GameId == gameId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameRosters for" +
      //                                            " GameTeam.GameId:" + gameId
      //                                    );
      //  }

      //  return found;
      //}

      //public List<GameRoster> FindGameRostersWithGameTeamId(int gameTeamId)
      //{
      //  return FindGameRostersWithGameTeamId(errorIfNotFound: true, gameTeamId: gameTeamId);
      //}

      //public List<GameRoster> FindGameRostersWithGameTeamId(bool errorIfNotFound, int gameTeamId)
      //{
      //  var found = _context.GameRosters
      //                    .Include("GameTeam")
      //                    .Include("GameTeam.Game")
      //                    .Include("GameTeam.Game.Season")
      //                    .Include("GameTeam.SeasonTeam")
      //                    .Include("GameTeam.SeasonTeam.Season")
      //                    .Include("GameTeam.SeasonTeam.Team")
      //                    .Include("GameTeam.SeasonTeam.Team.Coach")
      //                    .Include("GameTeam.SeasonTeam.Team.Sponsor")

      //                    .Include("Player")
      //                    .Include("SubbingForPlayer")
      //                    .Where(x => x.GameTeamId == gameTeamId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameRosters for" +
      //                                            " GameTeamId:" + gameTeamId
      //                                    );
      //  }

      //  return found;
      //}
      #endregion

      #region GameScore Functions TODO
      //#region Find-GameScore
      //public GameScore FindGameScoreByPK2(int gameTeamId, int period)
      //{
      //  return FindGameScoreByPK2(errorIfNotFound: true, errorIfMoreThanOneFound: true, gameTeamId: gameTeamId, period: period);
      //}

      //public GameScore FindGameScoreByPK2(bool errorIfNotFound, bool errorIfMoreThanOneFound, int gameTeamId, int period)
      //{
      //  var found = _context.GameScores.Where(x => x.GameTeamId == gameTeamId && x.Period == period).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameScore for" +
      //                                            " GameTeamId:" + gameTeamId +
      //                                            " Period:" + period
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 GameScore was not found for" +
      //                                            " GameTeamId:" + gameTeamId +
      //                                            " Period:" + period
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //public GameScore FindGameScore(int gameScoreId)
      //{
      //  return FindGameScore(errorIfNotFound: true, errorIfMoreThanOneFound: true, gameScoreId: gameScoreId);
      //}

      //public GameScore FindGameScore(bool errorIfNotFound, bool errorIfMoreThanOneFound, int gameScoreId)
      //{
      //  var found = _context.GameScores.Where(x => x.GameScoreId == gameScoreId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find GameScore for" +
      //                                            " GameScoreId:" + gameScoreId
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 GameScore was not found for" +
      //                                            " GameScoreId:" + gameScoreId
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-GameScore
      //public int SaveOrUpdateGameScore(List<GameScore> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateGameScore(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateGameScore(GameScore toSave)
      //{
      //  GameScore found;
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;

      //  // lookup by PK, if it exists
      //  if (toSave.GameScoreId > 0)
      //  {
      //    found = FindGameScore(errorIfNotFound, errorIfMoreThanOneFound, toSave.GameScoreId);
      //  }
      //  else
      //  {
      //    // lookup by PK2
      //    found = FindGameScoreByPK2(errorIfNotFound, errorIfMoreThanOneFound, toSave.GameTeamId, toSave.Period);

      //    // if found, set missing PK
      //    if (found != null)
      //    {
      //      toSave.GameScoreId = found.GameScoreId;
      //    }
      //  }

      //  if (found == null)
      //  {
      //    _context.GameScores.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region GameTeam Functions (ByPK and PK2, New format)
      #region Find-GameTeam
      public GameTeam FindGameTeamByPK2(int gameId, bool homeTeam, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      {
        return FindGameTeamByPK2(errorIfNotFound: true, errorIfMoreThanOneFound: true, gameId: gameId, homeTeam: homeTeam);

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

      #region GoalieStatGame Functions TODO
      //#region Find-GoalieStatGame NEW FORMAT
      //public GoalieStatGame FindGoalieStatGame(int playerId, int gameId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<GoalieStatGame, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                      x.GameId == gameId
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " GameId:" + gameId
      //                            ;

      //  return FindGoalieStatGame(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private GoalieStatGame FindGoalieStatGame(Expression<Func<GoalieStatGame, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<GoalieStatGame> found;

      //  if (populateFully)
      //  {
      //    found = _context.GoalieStatsGame
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Include("SeasonTeam")
      //                        .Include("SeasonTeam.Season")
      //                        .Include("SeasonTeam.Team")
      //                        .Include("SeasonTeam.Team.Coach")
      //                        .Include("SeasonTeam.Team.Sponsor")
      //                        .Include("Game")
      //                        .Include("Game.Season")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.GoalieStatsGame.Where(whereClause).ToList();
      //  }

      //  return FindBase<GoalieStatGame>(found, "GoalieStatGame", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-GoalieStatGame
      //public int SaveOrUpdateGoalieStatGame(List<GoalieStatGame> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateGoalieStatGame(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateGoalieStatGame(GoalieStatGame toSave)
      //{
      //  GoalieStatGame found = FindGoalieStatGame(toSave.PlayerId, toSave.GameId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    _context.GoalieStatsGame.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region GoalieStatSeason Functions TODO
      //#region Find-GoalieStatSeason NEW FORMAT
      //public GoalieStatSeason FindGoalieStatSeason(int playerId, int seasonId, bool playoffs, bool sub, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<GoalieStatSeason, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                        x.SeasonId == seasonId &&
      //                                                        x.Playoffs == playoffs &&
      //                                                        x.Sub == sub
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " SeasonId:" + seasonId +
      //                            " Playoffs:" + playoffs +
      //                            " Sub:" + sub
      //                            ;

      //  return FindGoalieStatSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private GoalieStatSeason FindGoalieStatSeason(Expression<Func<GoalieStatSeason, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<GoalieStatSeason> found;

      //  if (populateFully)
      //  {
      //    found = _context.GoalieStatsSeason
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.GoalieStatsSeason.Where(whereClause).ToList();
      //  }

      //  return FindBase<GoalieStatSeason>(found, "GoalieStatSeason", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-GoalieStatSeason
      //public int SaveOrUpdateGoalieStatSeason(List<GoalieStatSeason> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateGoalieStatSeason(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateGoalieStatSeason(GoalieStatSeason toSave)
      //{
      //  GoalieStatSeason found = FindGoalieStatSeason(toSave.PlayerId, toSave.SeasonId, toSave.Playoffs, toSave.Sub, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    _context.GoalieStatsSeason.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region GoalieStatSeasonTeam Functions TODO
      //#region Find-GoalieStatSeasonTeam NEW FORMAT
      //public GoalieStatSeasonTeam FindGoalieStatSeasonTeam(int playerId, int seasonTeamIdPlayingFor, bool playoffs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<GoalieStatSeasonTeam, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                        x.SeasonTeamId == seasonTeamIdPlayingFor &&
      //                                                        x.Playoffs == playoffs
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " SeasonTeamId:" + seasonTeamIdPlayingFor +
      //                            " Playoffs:" + playoffs
      //                            ;

      //  return FindGoalieStatSeasonTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private GoalieStatSeasonTeam FindGoalieStatSeasonTeam(Expression<Func<GoalieStatSeasonTeam, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<GoalieStatSeasonTeam> found;

      //  if (populateFully)
      //  {
      //    found = _context.GoalieStatsSeasonTeam
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Include("SeasonTeam")
      //                        .Include("SeasonTeam.Season")
      //                        .Include("SeasonTeam.Team")
      //                        .Include("SeasonTeam.Team.Coach")
      //                        .Include("SeasonTeam.Team.Sponsor")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.GoalieStatsSeasonTeam.Where(whereClause).ToList();
      //  }

      //  return FindBase<GoalieStatSeasonTeam>(found, "GoalieStatSeasonTeam", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-GoalieStatSeasonTeam
      //public int SaveOrUpdateGoalieStatSeasonTeam(List<GoalieStatSeasonTeam> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateGoalieStatSeasonTeam(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateGoalieStatSeasonTeam(GoalieStatSeasonTeam toSave)
      //{
      //  GoalieStatSeasonTeam found = FindGoalieStatSeasonTeam(toSave.PlayerId, toSave.SeasonTeamId, toSave.Playoffs, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    _context.GoalieStatsSeasonTeam.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region Penalty Functions  TODO

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

      #region Player Functions 
      #region Find-Player
      public Player FindPlayer(int playerId)
      {
        return FindPlayer(errorIfNotFound: true, errorIfMoreThanOneFound: true, playerId: playerId);
      }

      public Player FindPlayer(bool errorIfNotFound, bool errorIfMoreThanOneFound, int playerId)
      {
        var found = _context.Players.Where(x => x.PlayerId == playerId).ToList();

        if (errorIfNotFound == true && found.Count < 1)
        {
          throw new ArgumentNullException("found", "Could not find Player for" +
                                                  " PlayerId:" + playerId
                                          );
        }

        if (errorIfMoreThanOneFound == true && found.Count > 1)
        {
          throw new ArgumentNullException("found", "More than 1 Player was not found for" +
                                                  " PlayerId:" + playerId
                                          );
        }

        if (found.Count == 1)
        {
          return found[0];
        }
        else
        {
          return null;
        }
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
        bool errorIfNotFound = false;
        bool errorIfMoreThanOneFound = true;
        Player found = FindPlayer(errorIfNotFound, errorIfMoreThanOneFound, toSave.PlayerId);

        if (found == null)
        {
          found = _context.Players.Add(toSave);
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

      #region PlayerStatCareer Functions TODO
      //#region Find-PlayerStatCareer NEW FORMAT
      //public PlayerStatCareer FindPlayerStatCareer(int playerId, bool playoffs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatCareer, bool>> whereClause = x => x.PlayerId == playerId && x.Playoffs == playoffs;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId + " Playoffs:" + playoffs;

      //  return FindPlayerStatCareer(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private PlayerStatCareer FindPlayerStatCareer(Expression<Func<PlayerStatCareer, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<PlayerStatCareer> found;

      //  if (populateFully)
      //  {
      //    found = _context.PlayerStatsCareer
      //                        .Include("Player")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.PlayerStatsCareer.Where(whereClause).ToList();
      //  }

      //  return FindBase<PlayerStatCareer>(found, "PlayerStatCareer", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-PlayerStatCareer
      //public int SaveOrUpdatePlayerStatCareer(List<PlayerStatCareer> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdatePlayerStatCareer(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdatePlayerStatCareer(PlayerStatCareer toSave)
      //{
      //  PlayerStatCareer found = FindPlayerStatCareer(toSave.PlayerId, toSave.Playoffs, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    found = _context.PlayerStatsCareer.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region PlayerStatGame Functions TODO
      //#region Find-PlayerStatGame NEW FORMAT
      //public PlayerStatGame FindPlayerStatGame(int playerId, int gameId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatGame, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                      x.GameId == gameId
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " GameId:" + gameId
      //                            ;

      //  return FindPlayerStatGame(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private PlayerStatGame FindPlayerStatGame(Expression<Func<PlayerStatGame, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<PlayerStatGame> found;

      //  if (populateFully)
      //  {
      //    found = _context.PlayerStatsGame
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Include("SeasonTeam")
      //                        .Include("SeasonTeam.Season")
      //                        .Include("SeasonTeam.Team")
      //                        .Include("SeasonTeam.Team.Coach")
      //                        .Include("SeasonTeam.Team.Sponsor")
      //                        .Include("Game")
      //                        .Include("Game.Season")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.PlayerStatsGame.Where(whereClause).ToList();
      //  }

      //  return FindBase<PlayerStatGame>(found, "PlayerStatGame", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-PlayerStatGame
      //public int SaveOrUpdatePlayerStatGame(List<PlayerStatGame> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdatePlayerStatGame(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdatePlayerStatGame(PlayerStatGame toSave)
      //{
      //  PlayerStatGame found = FindPlayerStatGame(toSave.PlayerId, toSave.GameId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    found = _context.PlayerStatsGame.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region PlayerStatSeason Functions TODO
      //#region Find-PlayerStatSeason NEW FORMAT
      //public PlayerStatSeason FindPlayerStatSeason(int playerId, int seasonId, bool playoffs, bool sub, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatSeason, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                        x.SeasonId == seasonId &&
      //                                                        x.Playoffs == playoffs &&
      //                                                        x.Sub == sub
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " SeasonId:" + seasonId +
      //                            " Playoffs:" + playoffs +
      //                            " Sub:" + sub
      //                            ;

      //  return FindPlayerStatSeason(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private PlayerStatSeason FindPlayerStatSeason(Expression<Func<PlayerStatSeason, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<PlayerStatSeason> found;

      //  if (populateFully)
      //  {
      //    found = _context.PlayerStatsSeason
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.PlayerStatsSeason.Where(whereClause).ToList();
      //  }

      //  return FindBase<PlayerStatSeason>(found, "PlayerStatSeason", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-PlayerStatSeason
      //public int SaveOrUpdatePlayerStatSeason(List<PlayerStatSeason> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdatePlayerStatSeason(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdatePlayerStatSeason(PlayerStatSeason toSave)
      //{
      //  PlayerStatSeason found = FindPlayerStatSeason(toSave.PlayerId, toSave.SeasonId, toSave.Playoffs, toSave.Sub, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    found = _context.PlayerStatsSeason.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region PlayerStatSeasonTeam Functions TODO
      //#region Find-PlayerStatSeasonTeam NEW FORMAT
      //public PlayerStatSeasonTeam FindPlayerStatSeasonTeam(int playerId, int seasonTeamIdPlayingFor, bool playoffs, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatSeasonTeam, bool>> whereClause = x => x.PlayerId == playerId &&
      //                                                        x.Playoffs == playoffs &&
      //                                                        x.SeasonTeamId == seasonTeamIdPlayingFor
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerId:" + playerId +
      //                            " Playoffs:" + playoffs +
      //                            " SeasonTeamId:" + seasonTeamIdPlayingFor
      //                            ;

      //  return FindPlayerStatSeasonTeam(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private PlayerStatSeasonTeam FindPlayerStatSeasonTeam(Expression<Func<PlayerStatSeasonTeam, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<PlayerStatSeasonTeam> found;

      //  if (populateFully)
      //  {
      //    found = _context.PlayerStatsSeasonTeam
      //                        .Include("Player")
      //                        .Include("Season")
      //                        .Include("SeasonTeam")
      //                        .Include("SeasonTeam.Season")
      //                        .Include("SeasonTeam.Team")
      //                        .Include("SeasonTeam.Team.Coach")
      //                        .Include("SeasonTeam.Team.Sponsor")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.PlayerStatsSeasonTeam.Where(whereClause).ToList();
      //  }

      //  return FindBase<PlayerStatSeasonTeam>(found, "PlayerStatSeasonTeam", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

      //#endregion

      //#region SaveOrUpdate-PlayerStatSeasonTeam
      //public int SaveOrUpdatePlayerStatSeasonTeam(List<PlayerStatSeasonTeam> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdatePlayerStatSeasonTeam(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdatePlayerStatSeasonTeam(PlayerStatSeasonTeam toSave)
      //{
      //  PlayerStatSeasonTeam found = FindPlayerStatSeasonTeam(toSave.PlayerId, toSave.SeasonTeamId, toSave.Playoffs, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

      //  if (found == null)
      //  {
      //    found = _context.PlayerStatsSeasonTeam.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region ScoreSheetEntry Functions (ByPK, New Format)
      #region Find-ScoreSheetEntry

      public ScoreSheetEntry FindScoreSheetEntry(int scoreSheetEntryId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      {
        Expression<Func<ScoreSheetEntry, bool>> whereClause = x => x.ScoreSheetEntryId == scoreSheetEntryId;

        string errMsgNotFoundFor = " ScoreSheetEntryId:" + scoreSheetEntryId
                                  ;

        return FindScoreSheetEntry(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      }

      private ScoreSheetEntry FindScoreSheetEntry(Expression<Func<ScoreSheetEntry, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      {
        List<ScoreSheetEntry> found;

        if (populateFully)
        {
          found = _context.ScoreSheetEntries
                              .Where(whereClause)
                              .ToList();
        }
        else
        {
          found = _context.ScoreSheetEntries.Where(whereClause).ToList();
        }

        return FindBase<ScoreSheetEntry>(found, "ScoreSheetEntry", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      }

      #endregion

      #region SaveOrUpdate-ScoreSheetEntry
      public int SaveOrUpdateScoreSheetEntry(List<ScoreSheetEntry> listToSave)
      {
        var saved = 0;
        foreach (var toSave in listToSave)
        {
          var results = SaveOrUpdateScoreSheetEntry(toSave);
          saved = saved + results;
        }

        return saved;
      }

      public int SaveOrUpdateScoreSheetEntry(ScoreSheetEntry toSave)
      {
        ScoreSheetEntry found = FindScoreSheetEntry(toSave.ScoreSheetEntryId, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);

        if (found == null)
        {
          _context.ScoreSheetEntries.Add(toSave);
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

      #region ScoreSheetEntryProcessed Functions TODO
      //#region Find-ScoreSheetEntryProcessed
      //public ScoreSheetEntryProcessed FindScoreSheetEntryProcessed(int scoreSheetEntryId)
      //{
      //  return FindScoreSheetEntryProcessed(errorIfNotFound: true, errorIfMoreThanOneFound: true, scoreSheetEntryId: scoreSheetEntryId);
      //}

      //public ScoreSheetEntryProcessed FindScoreSheetEntryProcessed(bool errorIfNotFound, bool errorIfMoreThanOneFound, int scoreSheetEntryId)
      //{
      //  var found = _context.ScoreSheetEntriesProcessed.Where(x => x.ScoreSheetEntryId == scoreSheetEntryId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ScoreSheetEntryProcessed for" +
      //                                            " scoreSheetEntryId:" + scoreSheetEntryId
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ScoreSheetEntryProcessed was not found for" +
      //                                            " scoreSheetEntryId:" + scoreSheetEntryId
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ScoreSheetEntryProcessed
      //public int SaveOrUpdateScoreSheetEntryProcessed(List<ScoreSheetEntryProcessed> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateScoreSheetEntryProcessed(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateScoreSheetEntryProcessed(ScoreSheetEntryProcessed toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ScoreSheetEntryProcessed found = FindScoreSheetEntryProcessed(errorIfNotFound, errorIfMoreThanOneFound, toSave.ScoreSheetEntryId);

      //  if (found == null)
      //  {
      //    found = _context.ScoreSheetEntriesProcessed.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region ScoreSheetEntryPenaltyProcessed Functions TODO
      //#region Find-ScoreSheetEntryPenaltyProcessed
      //public ScoreSheetEntryPenaltyProcessed FindScoreSheetEntryPenaltyProcessed(int scoreSheetEntryPenaltyId)
      //{
      //  return FindScoreSheetEntryPenaltyProcessed(errorIfNotFound: true, errorIfMoreThanOneFound: true, scoreSheetEntryPenaltyId: scoreSheetEntryPenaltyId);
      //}

      //public ScoreSheetEntryPenaltyProcessed FindScoreSheetEntryPenaltyProcessed(bool errorIfNotFound, bool errorIfMoreThanOneFound, int scoreSheetEntryPenaltyId)
      //{
      //  var found = _context.ScoreSheetEntryPenaltiesProcessed.Where(x => x.ScoreSheetEntryPenaltyId == scoreSheetEntryPenaltyId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ScoreSheetEntryPenaltyProcessed for" +
      //                                            " scoreSheetEntryPenaltyId:" + scoreSheetEntryPenaltyId
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ScoreSheetEntryPenaltyProcessed was not found for" +
      //                                            " scoreSheetEntryPenaltyId:" + scoreSheetEntryPenaltyId
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ScoreSheetEntryPenaltyProcessed
      //public int SaveOrUpdateScoreSheetEntryPenaltyProcessed(List<ScoreSheetEntryPenaltyProcessed> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateScoreSheetEntryPenaltyProcessed(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateScoreSheetEntryPenaltyProcessed(ScoreSheetEntryPenaltyProcessed toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ScoreSheetEntryPenaltyProcessed found = FindScoreSheetEntryPenaltyProcessed(errorIfNotFound, errorIfMoreThanOneFound, toSave.ScoreSheetEntryPenaltyId);

      //  if (found == null)
      //  {
      //    _context.ScoreSheetEntryPenaltiesProcessed.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
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

      #region ScoreSheetEntrySubProcessed Functions TODO
      //#region Find-ScoreSheetEntrySubProcessed
      //public ScoreSheetEntrySubProcessed FindScoreSheetEntrySubProcessed(int scoreSheetEntrySubId)
      //{
      //  return FindScoreSheetEntrySubProcessed(errorIfNotFound: true, errorIfMoreThanOneFound: true, scoreSheetEntrySubId: scoreSheetEntrySubId);
      //}

      //public ScoreSheetEntrySubProcessed FindScoreSheetEntrySubProcessed(bool errorIfNotFound, bool errorIfMoreThanOneFound, int scoreSheetEntrySubId)
      //{
      //  var found = _context.ScoreSheetEntrySubsProcessed.Where(x => x.ScoreSheetEntrySubId == scoreSheetEntrySubId).ToList();

      //  if (errorIfNotFound == true && found.Count < 1)
      //  {
      //    throw new ArgumentNullException("found", "Could not find ScoreSheetEntrySubProcessed for" +
      //                                            " scoreSheetEntrySubId:" + scoreSheetEntrySubId
      //                                    );
      //  }

      //  if (errorIfMoreThanOneFound == true && found.Count > 1)
      //  {
      //    throw new ArgumentNullException("found", "More than 1 ScoreSheetEntrySubProcessed was found for" +
      //                                            " scoreSheetEntrySubId:" + scoreSheetEntrySubId
      //                                    );
      //  }

      //  if (found.Count == 1)
      //  {
      //    return found[0];
      //  }
      //  else
      //  {
      //    return null;
      //  }
      //}
      //#endregion

      //#region SaveOrUpdate-ScoreSheetEntrySubProcessed
      //public int SaveOrUpdateScoreSheetEntrySubProcessed(List<ScoreSheetEntrySubProcessed> listToSave)
      //{
      //  var saved = 0;
      //  foreach (var toSave in listToSave)
      //  {
      //    var results = SaveOrUpdateScoreSheetEntrySubProcessed(toSave);
      //    saved = saved + results;
      //  }

      //  return saved;
      //}

      //public int SaveOrUpdateScoreSheetEntrySubProcessed(ScoreSheetEntrySubProcessed toSave)
      //{
      //  bool errorIfNotFound = false;
      //  bool errorIfMoreThanOneFound = true;
      //  ScoreSheetEntrySubProcessed found = FindScoreSheetEntrySubProcessed(errorIfNotFound, errorIfMoreThanOneFound, toSave.ScoreSheetEntrySubId);

      //  if (found == null)
      //  {
      //    found = _context.ScoreSheetEntrySubsProcessed.Add(toSave);
      //  }
      //  else
      //  {
      //    var entry = _context.Entry(found);
      //    entry.OriginalValues.SetValues(found);
      //    entry.CurrentValues.SetValues(toSave);
      //  }

      //  return ContextSaveChanges();
      //}
      //#endregion
      #endregion

      #region Season Functions
      #region Find-Season (addtl finds)
      public Season FindSeasonWithYYYYMMDD(int yyyymmdd)
      {
        return FindSeasonWithYYYYMMDD(errorIfNotFound: true, errorIfMoreThanOneFound: true, yyyymmdd: yyyymmdd);
      }

      public Season FindSeasonWithYYYYMMDD(bool errorIfNotFound, bool errorIfMoreThanOneFound, int yyyymmdd)
      {
        var found = _context.Seasons.Where(x => x.StartYYYYMMDD >= yyyymmdd && x.EndYYYYMMDD <= yyyymmdd).ToList();

        if (errorIfNotFound == true && found.Count < 1)
        {
          throw new ArgumentNullException("found", "Could not find Season for" +
                                                  " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                          );
        }

        if (errorIfMoreThanOneFound == true && found.Count > 1)
        {
          throw new ArgumentNullException("found", "More than 1 Season was not found for" +
                                                  " StartYYYYMMDD and EndYYYYMMDD between:" + yyyymmdd
                                          );
        }

        if (found.Count == 1)
        {
          return found[0];
        }
        else
        {
          return null;
        }
      }

      public Season FindSeasonWithIsCurrentSeason(bool isCurrentSeason)
      {
        return FindSeasonCurrent(errorIfNotFound: true, errorIfMoreThanOneFound: true, isCurrentSeason: isCurrentSeason);
      }

      public Season FindSeasonCurrent(bool errorIfNotFound, bool errorIfMoreThanOneFound, bool isCurrentSeason)
      {
        var found = _context.Seasons.Where(x => x.IsCurrentSeason == isCurrentSeason).ToList();

        if (errorIfNotFound == true && found.Count < 1)
        {
          throw new ArgumentNullException("found", "Could not find Season for" +
                                                  " isCurrentSeason:" + isCurrentSeason
                                          );
        }

        if (errorIfMoreThanOneFound == true && found.Count > 1)
        {
          throw new ArgumentNullException("found", "More than 1 Season was not found for" +
                                                  " isCurrentSeason:" + isCurrentSeason
                                          );
        }

        if (found.Count == 1)
        {
          return found[0];
        }
        else
        {
          return null;
        }
      }

      public Season FindSeason(int seasonId)
      {
        return FindSeason(errorIfNotFound: true, errorIfMoreThanOneFound: true, seasonId: seasonId);
      }

      public Season FindSeason(bool errorIfNotFound, bool errorIfMoreThanOneFound, int seasonId)
      {
        var found = _context.Seasons.Where(x => x.SeasonId == seasonId).ToList();

        if (errorIfNotFound == true && found.Count < 1)
        {
          throw new ArgumentNullException("found", "Could not find Season for" +
                                                  " SeasonId:" + seasonId
                                          );
        }

        if (errorIfMoreThanOneFound == true && found.Count > 1)
        {
          throw new ArgumentNullException("found", "More than 1 Season was not found for" +
                                                  " SeasonId:" + seasonId
                                          );
        }

        if (found.Count == 1)
        {
          return found[0];
        }
        else
        {
          return null;
        }
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
        bool errorIfNotFound = false;
        bool errorIfMoreThanOneFound = true;
        Season found = FindSeason(errorIfNotFound, errorIfMoreThanOneFound, toSave.SeasonId);

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

      #region Settings Functions
      #region Find-Setting
      public Setting FindSetting(int settingId)
      {
        bool errorIfNotFound = true;
        bool errorIfMoreThanOneFound = true;
        return FindSetting(errorIfNotFound, errorIfMoreThanOneFound, settingId);
      }

      public Setting FindSetting(bool errorIfNotFound, bool errorIfMoreThanOneFound, int settingId)
      {
        var found = _context.Settings.Where(x => x.SettingId == settingId).ToList();

        if (errorIfNotFound == true && found.Count < 1)
        {
          throw new ArgumentNullException("found", "Could not find Setting for" +
                                                  " SettingId:" + settingId
                                          );
        }

        if (errorIfMoreThanOneFound == true && found.Count > 1)
        {
          throw new ArgumentNullException("found", "More than 1 Setting was not found for" +
                                                  " SettingId:" + settingId
                                          );
        }

        if (found.Count == 1)
        {
          return found[0];
        }
        else
        {
          return null;
        }
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
        bool errorIfNotFound = false;
        bool errorIfMoreThanOneFound = true;
        Setting found = FindSetting(errorIfNotFound, errorIfMoreThanOneFound, toSave.SettingId);

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

      #region TeamRoster Functions
      #region Find-TeamRoster(s) (addtl finds)  NEW FORMAT
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

      #region Teams
      #region Find-Team NEW FORMAT
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


      #region Find-PlayerStatus (addtl finds) NEW FORMAT TODO
      //public PlayerStatus FindPlayerStatusWithCurrentStatus(int playerStatusId, bool currentStatus, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatus, bool>> whereClause = x => x.PlayerStatusId == playerStatusId &&
      //                                                       x.CurrentStatus == currentStatus
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerStatusId:" + playerStatusId +
      //                            " CurrentStatus:" + currentStatus
      //                            ;

      //  return FindPlayerStatus(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //public PlayerStatus FindPlayerStatus(int playerStatusId, bool errorIfNotFound = true, bool errorIfMoreThanOneFound = true, bool populateFully = false)
      //{
      //  Expression<Func<PlayerStatus, bool>> whereClause = x => x.PlayerStatusId == playerStatusId
      //                                                      ;

      //  string errMsgNotFoundFor = " PlayerStatusId:" + playerStatusId
      //                            ;

      //  return FindPlayerStatus(whereClause, errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound, populateFully);
      //}

      //private PlayerStatus FindPlayerStatus(Expression<Func<PlayerStatus, bool>> whereClause, string errMsgNotFoundFor, bool errorIfNotFound, bool errorIfMoreThanOneFound, bool populateFully)
      //{
      //  List<PlayerStatus> found;

      //  if (populateFully)
      //  {
      //    found = _context.PlayerStatuses
      //                        .Include("Player")
      //                        .Include("PlayerStatusType")
      //                        .Where(whereClause)
      //                        .ToList();
      //  }
      //  else
      //  {
      //    found = _context.PlayerStatuses.Where(whereClause).ToList();
      //  }

      //  return FindBase<PlayerStatus>(found, "PlayerStatus", errMsgNotFoundFor, errorIfNotFound, errorIfMoreThanOneFound);
      //}

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