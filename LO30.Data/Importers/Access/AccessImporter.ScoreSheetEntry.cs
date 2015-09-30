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
    public ImportStat ImportScoreSheetEntries()
    {
      string table = "ScoreSheetEntries";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.ScoreSheetEntryGoals.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "ScoreSheetEntries.json");
        int count = parsedJson.Count;
        int countSaveOrUpdated = 0;

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("ImportScoreSheetEntries: Access records processed:" + d + ". Records saved or updated:" + countSaveOrUpdated); }

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

            var scoreSheetEntry = new ScoreSheetEntryGoal()
            {
              ScoreSheetEntryGoalId = json["SCORE_SHEET_ENTRY_ID"],
              GameId = gameId,
              Period = json["PERIOD"],
              HomeTeam = homeTeam,
              Goal = json["GOAL"],
              Assist1 = json["ASSIST1"],
              Assist2 = json["ASSIST2"],
              Assist3 = json["ASSIST3"],
              TimeRemaining = json["TIME_REMAINING"],
              ShortHandedPowerPlay = json["SH_PP"],
              UpdatedOn = updatedOn
            };

            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateScoreSheetEntryGoal(scoreSheetEntry);

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