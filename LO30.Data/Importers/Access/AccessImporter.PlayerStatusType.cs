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
    public ImportStat ImportPlayerStatusTypes()
    {
      string table = "PlayerStatusTypes";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.PlayerStatusTypes.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Statuses.json");
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          var playerStatusType = new PlayerStatusType()
          {
            PlayerStatusTypeId = json["STATUS_ID"],
            PlayerStatusTypeName = json["STATUS_DESC"]
          };

          _context.PlayerStatusTypes.Add(playerStatusType);
        }

        _lo30ContextService.ContextSaveChanges();
        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.PlayerStatusTypes.Count());
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