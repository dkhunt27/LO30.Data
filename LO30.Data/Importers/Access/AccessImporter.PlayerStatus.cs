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
    public ImportStat ImportPlayerStatuses()
    {
      string table = "PlayerStatuses";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.PlayerStatuses.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "PlayerStatuses.json");
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          int playerId = json["PLAYER_ID"];

          if (playerId == 512 || playerId == 545 || playerId == 571 || playerId == 170 || playerId == 211 || playerId == 213 || playerId == 215 || playerId == 217 || playerId == 282 || playerId == 381 || playerId == 426 || playerId == 432 || playerId == 767)
          {
            // do nothing, these guys do not have a player record
          }
          else
          {
            DateTime? startDate = json["START_DATE"];
            DateTime? endDate = json["END_DATE"];
            int startYYYYMMDD = ConvertDateTimeIntoYYYYMMDD(startDate, ifNullReturnMax: false);
            int endYYYYMMDD = ConvertDateTimeIntoYYYYMMDD(endDate, ifNullReturnMax: true);

            var playerStatus = new PlayerStatus()
            {
              PlayerId = playerId,
              PlayerStatusTypeId = json["STATUS_ID"],
              StartYYYYMMDD = startYYYYMMDD,
              EndYYYYMMDD = endYYYYMMDD,
              CurrentStatus = false
            };

            _context.PlayerStatuses.Add(playerStatus);
          }
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.PlayerStatuses.Count());

        // determine the current status
        var currentPlayerStatus = _context.PlayerStatuses
                                            .GroupBy(x => new { x.PlayerId })
                                            .Select(grp => new
                                            {
                                              PlayerId = grp.Key.PlayerId,
                                              EndYYYYMMDD = grp.Max(x => x.EndYYYYMMDD)
                                            })
                                            .ToList();

        // now update those records
        foreach (var current in currentPlayerStatus)
        {
          var playerStatus = _context.PlayerStatuses.Where(x => x.PlayerId == current.PlayerId && x.EndYYYYMMDD == current.EndYYYYMMDD).FirstOrDefault();
          playerStatus.CurrentStatus = true;
        }

        int updated = _lo30ContextService.ContextSaveChanges();
        _logger.Write("Data Group 3: Updated PlayerStatuses Current " + updated);
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