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
    public ImportStat ImportPlayerDrafts()
    {
      string table = "PlayerDrafts";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.PlayerDrafts.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "PlayerRatings.json");
        int count = parsedJson.Count;
        int countSaveOrUpdated = 0;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          int seasonId = json["SEASON_ID"];
          int playerId = json["PLAYER_ID"];

          string draftRound = "";
          string position = "";
          if (json["PLAYER_DRAFT_ROUND"] != null)
          {
            draftRound = json["PLAYER_DRAFT_ROUND"];
          }

          int round = -1;
          int line = -1;

          switch (draftRound.ToLower())
          {
            case "g":
            case "1g":
            case "2g":
            case "3g":
            case "4g":
            case "5g":
            case "6g":
            case "7g":
            case "8g":
              position = "G";
              line = 1;
              round = 1;
              break;
            case "1d":
              position = "D";
              line = 1;
              round = 2;
              break;
            case "2d":
              position = "D";
              line = 1;
              round = 5;
              break;
            case "3d":
              position = "D";
              line = 2;
              round = 8;
              break;
            case "4d":
              position = "D";
              line = 2;
              round = 11;
              break;
            case "5d":
              position = "D";
              line = 3;
              break;
            case "6d":
              position = "D";
              line = 3;
              round = 14;
              break;
            case "1f":
              position = "F";
              line = 1;
              round = 3;
              break;
            case "2f":
              position = "F";
              line = 1;
              round = 4;
              break;
            case "3f":
              position = "F";
              line = 1;
              round = 6;
              break;
            case "4f":
              position = "F";
              line = 2;
              round = 7;
              break;
            case "5f":
              position = "F";
              line = 2;
              round = 9;
              break;
            case "6f":
              position = "F";
              line = 2;
              round = 10;
              break;
            case "7f":
              position = "F";
              line = 3;
              round = 12;
              break;
            case "8f":
              position = "F";
              line = 3;
              round = 13;
              break;
            case "9f":
              position = "F";
              line = 3;
              round = 15;
              break;
          }
          if (playerId == 545 || playerId == 512 || playerId == 426 || playerId == 432 || playerId == 381 || playerId == 282)
          {
            // skip these players...they do not exist in the players table
          }
          else
          {
            var playerDraft = new PlayerDraft()
            {
              SeasonId = seasonId,
              PlayerId = playerId,
              Round = round,
              Order = -1,
              Position = position,
              Line = line,
              Special = false
            };

            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdatePlayerDraft(playerDraft);
          }
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.PlayerDrafts.Count());
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