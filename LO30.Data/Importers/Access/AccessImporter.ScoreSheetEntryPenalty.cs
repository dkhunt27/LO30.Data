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
    public ImportStat ImportScoreSheetEntryPenalties()
    {
      string table = "ScoreSheetEntryPenalty";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.ScoreSheetEntryPenalties.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "ScoreSheetEntryPenalties.json");
        int count = parsedJson.Count;
        int countSaveOrUpdated = 0;

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("ImportScoreSheetEntryPenalties: Access records processed:" + d + ". Records saved or updated:" + countSaveOrUpdated); }

          var json = parsedJson[d];
          int gameId = json["GAME_ID"];

          //if (gameId >= startingGameIdToProcess && gameId <= endingGameIdToProcess)
          {
            bool homeTeam = true;
            string teamJson = json["TEAM"];
            string team = teamJson.ToLower();
            if (team == "2" || team == "v" || team == "a" || team == "g")
            {
              homeTeam = false;
            }

            DateTime updatedOn = json["UPDATED_ON"];

            var scoreSheetEntryPenalty = new ScoreSheetEntryPenalty()
            {
              ScoreSheetEntryPenaltyId = json["SCORE_SHEET_ENTRY_PENALTY_ID"],
              GameId = json["GAME_ID"],
              Period = json["PERIOD"],
              HomeTeam = homeTeam,
              Player = json["PLAYER"],
              PenaltyCode = json["PENALTY_CODE"],
              TimeRemaining = json["TIME_REMAINING"],
              PenaltyMinutes = json["PENALTY_MINUTES"],
              UpdatedOn = updatedOn
            };

            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateScoreSheetEntryPenalty(scoreSheetEntryPenalty);

          }
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.ScoreSheetEntryGoals.Count());
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