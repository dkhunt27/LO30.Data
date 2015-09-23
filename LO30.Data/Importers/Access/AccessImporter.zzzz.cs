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
    public ImportStat ImportZzzzzz()
    {
      string table = "Zzzzz";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Seasons.Count() == 0)
      {
        _logger.Write("Importing " + table);


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