using LO30.Data.Contexts;
using LO30.Data.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LO30.Data.ContextHelper
{

  class Program
  {
    static void Main(string[] args)
    {
      //LoggingConfiguration loggingConfiguration = BuildProgrammaticConfig();
      //LogWriter defaultWriter = new LogWriter(loggingConfiguration);

      //LogWriterFactory logWriterFactory = new LogWriterFactory();
      //LogWriter logWriter = logWriterFactory.Create();

      LogService logService = new LogService();
      LogWriter logger = logService.CreateLogger();

      if (logger.IsLoggingEnabled())
      {
        logger.Write("Log entry created using the simplest overload.");
        logger.Write("Log entry with a single category.", "General");
        logger.Write("Log entry with a category, priority, and event ID.",
                            "General", 6, 9001);
        logger.Write("Log entry with a category, priority, event ID, "
                            + "and severity.", "General", 5, 9002,
                            TraceEventType.Warning);
        logger.Write("Log entry with a category, priority, event ID, "
                            + "severity, and title.", "General", 8, 9003,
                            TraceEventType.Warning, "Logging Block Examples");
      }
      else
      {
        Console.WriteLine("Logging is disabled in the configuration.");
      }

      //Database.SetInitializer(new LO30ContextSeedInitializer());

      using (var context = new LO30Context())
      {
        //context.Database.Initialize(force: true);

        // determine the current status
        var x = context.Seasons.ToList();
      }
    }
  }
}
   