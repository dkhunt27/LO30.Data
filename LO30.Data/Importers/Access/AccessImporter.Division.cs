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
    public ImportStat ImportDivisions()
    {
      string table = "Divisions";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Divisions.Count() == 0)
      {
        _logger.Write("Importing " + table);

        var division = new Division(did: 0, dln: "No Division", dsn: "n/a");
        int saveOrUpdatedCount = +_lo30ContextService.SaveOrUpdateDivision(division);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Teams.json");
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          string divName = json["TEAM_DIVISION_NAME"];

          if (!string.IsNullOrWhiteSpace(divName))
          {
            var found = _lo30ContextService.FindDivisionByPK2(divName, errorIfNotFound: false, errorIfMoreThanOneFound: true, populateFully: false);
            if (found == null)
            { // only add new divisions
              division = new Division()
              {
                DivisionLongName = divName,
                DivisionShortName = "TBD"
              };
              saveOrUpdatedCount = +_lo30ContextService.SaveOrUpdateDivision(division);
            }
          }
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Divisions.Count());
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