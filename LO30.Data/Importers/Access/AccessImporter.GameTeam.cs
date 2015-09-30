using LO30.Data.Models;
using LO30.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LO30.Data.Importers.Access
{
  public partial class AccessImporter
  {
    public ImportStat ImportGameTeams()
    {
      string table = "GameTeams";
      var iStat = new ImportStat(_logger, table);

            if (_seed && _context.GameTeams.Count() == 0)
            {
                _logger.Write("Importing " + table);

                dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Games.json");
                int count = parsedJson.Count;

                _logger.Write("ImportGameTeams: Access records to process:" + count);

                int countSaveOrUpdated = 0;
                for (var d = 0; d < parsedJson.Count; d++)
                {
                    if (d % 100 == 0) { _logger.Write("ImportGameTeams: Access records processed:" + d); }
                    var json = parsedJson[d];

                    int gameId = json["GAME_ID"];
                    int seasonId = json["SEASON_ID"];

                    int homeTeamId, awayTeamId;


                    homeTeamId = json["HOME_TEAM_ID"];
                    awayTeamId = json["AWAY_TEAM_ID"];


                    // FK check
                    //_lo30ContextService.FindGame(gameId, errorIfNotFound: true, errorIfMoreThanOneFound: true, populateFully: false);
                    //_lo30ContextService.FindTeam(homeTeamId, errorIfNotFound: true, errorIfMoreThanOneFound: true, populateFully: false);
                    //_lo30ContextService.FindTeam(awayTeamId, errorIfNotFound: true, errorIfMoreThanOneFound: true, populateFully: false);

                    var gameTeam = new GameTeam(sid: seasonId, gid: gameId, ht: true, tid: homeTeamId, otid: awayTeamId);
                    countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateGameTeam(gameTeam);

                    gameTeam = new GameTeam(sid: seasonId, gid: gameId, ht: false, tid: awayTeamId, otid: homeTeamId);

                    countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateGameTeam(gameTeam);
                }

                iStat.Imported();
                ContextSaveChanges();
                iStat.Saved(_context.GameTeams.Count());
            }
            else
            {
                _logger.Write(table + " records exist in context; not importing");
                iStat.Imported();
                iStat.Saved(0);
            }

            iStat.Log();

            return iStat;
        }
    }
}