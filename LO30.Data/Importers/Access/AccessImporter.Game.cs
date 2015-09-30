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
    public ImportStat ImportGames()
    {
      string table = "Games";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Games.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Games.json");
        int count = parsedJson.Count;

        _logger.Write("SaveOrUpdateGames:Access records to process:" + count);

        int countSaveOrUpdated = 0;
        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("SaveOrUpdateGames:Access records processed:" + d); }
          var json = parsedJson[d];

          int gameId = json["GAME_ID"];
          //if (gameId >= startingGameIdToProcess && gameId <= endingGameIdToProcess)
          //{
            int seasonId = json["SEASON_ID"];
            DateTime gameDate = json["GAME_DATE"];
            DateTime gameTime = json["GAME_TIME"];
            bool playoffGame = json["PLAYOFF_GAME_IND"];

            var timeSpan = new TimeSpan(gameTime.Hour, gameTime.Minute, gameTime.Second);

            var gameDateTime = gameDate.Add(timeSpan);

            var game = new Game(
                  sid: seasonId,
                  gid: gameId,
                  time: gameDateTime,
                  loc: "not set",
                  pfs: playoffGame
            );

            //context.Games.Add(game);  // works only if never reprocessing data

            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateGame(game);
          //}
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Games.Count());
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