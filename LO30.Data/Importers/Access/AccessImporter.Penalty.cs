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
    public ImportStat ImportPenalties()
    {
      string table = "Penalties";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Penalties.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(Path.Combine(_folderPath, "Penalties.json"));
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          var penalty = new Penalty()
          {
            PenaltyId = json["PENALTY_ID"],
            PenaltyCode = json["PENALTY_SHORT_DESC"],
            PenaltyName = json["PENALTY_LONG_DESC"],
            DefaultPenaltyMinutes = json["DEFAULT_PENALTY_MINUTES"],
            StickPenalty = json["STICK_PENALTY"]
          };

          _context.Penalties.Add(penalty);
        }


        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Penalties.Count());
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