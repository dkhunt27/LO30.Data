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
    public ImportStat ImportSeasons()
    {
      string table = "Seasons";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Seasons.Count() == 0)
      {
        _logger.Write("Importing " + table);

        #region add placeholder season
        var seasonIdPlaceholder = -1;
        var season = new Season(sid: seasonIdPlaceholder, sn: "Placeholder", ics: false, stymd: 0, endymd: 0);
        _context.Seasons.Add(season);
        #endregion


        dynamic parsedJson = _accessDatabaseService.ParseObjectFromJsonFile(_folderPath + "Seasons.json");
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { Console.WriteLine("Access records processed:" + d); }
          var json = parsedJson[d];

          DateTime? startDate = null;
          DateTime? endDate = null;

          if (json["START_DATE"] != null)
          {
            startDate = json["START_DATE"];
          }

          if (json["END_DATE"] != null)
          {
            endDate = json["END_DATE"];
          }

          int seasonId = json["SEASON_ID"];

          if (seasonId == 54)
          {
            startDate = new DateTime(2014, 9, 4);
            endDate = new DateTime(2015, 3, 29);
          }

          season = new Season(sid: seasonId, sn: json["SEASON_NAME"].ToString(), ics: Convert.ToBoolean(json["CURRENT_SEASON_IND"]), stymd: ConvertDateTimeIntoYYYYMMDD(startDate, ifNullReturnMax: false), endymd: ConvertDateTimeIntoYYYYMMDD(endDate, ifNullReturnMax: true));
          _context.Seasons.Add(season);
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Seasons.Count());
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
