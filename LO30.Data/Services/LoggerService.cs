using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace LO30.Data.Services
{
  public class LogService
  {
    public LogService()
    {
    }

    public LogWriter CreateLogger()
    {
      // Formatter
      TextFormatter briefFormatter = new TextFormatter("{timestamp}  {message} {title} {dictionary({key} - {value}{newline})}");

      // Trace Listener
      var flatFileTraceListener = new FlatFileTraceListener(
        @"C:\Temp\FlatFile.log", 
        "----------------------------------------", 
        "----------------------------------------", 
        briefFormatter);

      // Build Configuration
      var config = new LoggingConfiguration();

      config.AddLogSource("DiskFiles", SourceLevels.All, true)
        .AddTraceListener(flatFileTraceListener);

      return new LogWriter(config);
    }
  }
}
