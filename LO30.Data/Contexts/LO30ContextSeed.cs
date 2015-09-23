using LO30.Data.Importers.Access;
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

namespace LO30.Data.Contexts
{
  public class LO30ContextSeed
  {
    private LO30Context _context;

    public LO30ContextSeed(LO30Context context)
    {
      _context = context;
    }

    public void Seed()
    {
      var logService = new LogService();
      var aImporter = new AccessImporter(logService.CreateLogger(), _context);
      aImporter.ImportPlayerStatusTypes();
      aImporter.ImportPenalties();
      aImporter.ImportPlayers();
      aImporter.ImportSeasons();
      aImporter.ImportDivisions();
      aImporter.ImportTeams();
      aImporter.ImportGames();
      aImporter.ImportGameTeams();
      aImporter.ImportPlayerStatuses();
      aImporter.ImportPlayerDrafts();
      aImporter.ImportPlayerRatings();
      aImporter.ImportTeamRosters();
      aImporter.ImportGameRosters();
      aImporter.ImportScoreSheetEntries();
      aImporter.ImportScoreSheetEntryPenalties();
      aImporter.ImportScoreSheetEntrySubs(); 


    }
  }

}
