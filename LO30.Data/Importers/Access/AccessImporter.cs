using LO30.Data.Contexts;
using LO30.Data.Models;
using LO30.Data.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging;
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
    
    private AccessDatabaseService _accessDatabaseService = new AccessDatabaseService();
    private string _folderPath = @"C:\git\LO30.Data\LO30.Data\RawData\Access\";
    DateTime _first = DateTime.Now;
    DateTime _last = DateTime.Now;
    TimeSpan _diffFromFirst = new TimeSpan();
    TimeSpan _diffFromLast = new TimeSpan();

    private LogWriter _logger;
    private LO30Context _context;
    private LO30ContextService _lo30ContextService;
    private bool _seed;

    public AccessImporter(LogWriter logger, LO30Context context, bool seed = true)
    {
      _logger = logger;
      _context = context;
      _lo30ContextService = new LO30ContextService(context);
      _seed = seed;
    }
    
    private int ContextSaveChanges()
    {
      try
      {
        return _context.SaveChanges();
      }
      catch (DbEntityValidationException e)
      {
        foreach (var eve in e.EntityValidationErrors)
        {
          Debug.Print("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
          foreach (var ve in eve.ValidationErrors)
          {
            Debug.Print("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
          }
        }
        throw;
      
      }
      catch (Exception ex)
      {
        Debug.Print(ex.Message);
        var innerEx = ex.InnerException;

        while (innerEx != null)
        {
          Debug.Print("With inner exception of:");
          Debug.Print(innerEx.Message);

          innerEx = innerEx.InnerException;
        }

        Debug.Print(ex.StackTrace);

        throw ex;
      }
    }

    private int ConvertDateTimeIntoYYYYMMDD(DateTime? toConvert, bool ifNullReturnMax)
    {
      int result = -1;

      if (toConvert == null)
      {
        if (ifNullReturnMax)
        {
          result = GetMaxYYYYMMDD();
        }
        else
        {
          result = GetMinYYYYMMDD();
        }
      }
      else
      {
        result = (toConvert.Value.Year * 10000) + (toConvert.Value.Month * 100) + toConvert.Value.Day;
      }

      return result;
    }

    private int GetMinYYYYMMDD()
    {
      int result = 12345678;
      return result;
    }

    private int GetMaxYYYYMMDD()
    {
      int result = 87654321;
      return result;
    }
  }

}
