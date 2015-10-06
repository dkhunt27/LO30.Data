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

      string connString = _context.Database.Connection.ConnectionString;

      #region create sql views
      var viewFileNameList = new List<string>(){
          //"BoxScoresDetailView.sql",
          //"BoxScoresSummaryView.sql",
          "GameRostersView.sql",
          "GamesView.sql",
          "GameTeamsView.sql",
          "GoalieStatCareersView.sql",
          "GoalieStatGamesView.sql",
          "GoalieStatSeasonsView.sql",
          "GoalieStatTeamsView.sql",
          "PlayerStatCareersView.sql",
          "PlayerStatGamesView.sql",
          "PlayerStatSeasonsView.sql",
          "PlayerStatTeamsView.sql",
          "TeamRostersView.sql",
          "TeamStandingsView.sql"
        };

      foreach (var viewFileName in viewFileNameList)
      {
        var viewFullFilePath = @"D:\git\LO30.v3\LO30.Common\SqlAssets\Views";
        viewFullFilePath = Path.Combine(viewFullFilePath, viewFileName);
        string viewSql = File.ReadAllText(viewFullFilePath);
        using (SqlConnection connection = new SqlConnection(connString))
        {
          // first drop the view
          var viewName = "dbo." + viewFileName.Replace(".sql", "");
          var dropSql = "IF OBJECT_ID('" + viewName + "', 'V') IS NOT NULL DROP VIEW " + viewName;
          SqlCommand command = new SqlCommand(dropSql, connection);
          command.Connection.Open();
          command.ExecuteNonQuery();

          command = new SqlCommand(viewSql, connection);
          command.ExecuteNonQuery();
        }
      }

      #endregion

      #region create sql stored procs
      var spFileNameList = new List<string>(){
          "DeriveForWebGoalieStats.sql",
          "DeriveForWebPlayerStats.sql",
          "DeriveForWebTeamStandings.sql",
          "DeriveGameOutcomes.sql",
          "DeriveGameRosters.sql",
          "DeriveGameScores.sql",
          "DeriveGoalieStatsCareer.sql",
          "DeriveGoalieStatsGame.sql",
          "DeriveGoalieStatsSeason.sql",
          "DeriveGoalieStatsTeam.sql",
          "DerivePlayerStatsCareer.sql",
          "DerivePlayerStatsGame.sql",
          "DerivePlayerStatsSeason.sql",
          "DerivePlayerStatsTeam.sql",
          "DeriveScoreSheetEntryProcessedGoals.sql",
          "DeriveScoreSheetEntryProcessedPenalties.sql",
          "DeriveScoreSheetEntryProcessedSubs.sql",
          "DeriveTeamStandings.sql"
        };

      foreach (var spFileName in spFileNameList)
      {
        var spFullFilePath = @"D:\git\LO30.v3\LO30.Common\SqlAssets\StoredProcedures";
        spFullFilePath = Path.Combine(spFullFilePath, spFileName);
        string viewSql = File.ReadAllText(spFullFilePath);
        using (SqlConnection connection = new SqlConnection(connString))
        {
          // first drop the view
          var viewName = "dbo." + spFileName.Replace(".sql", "");
          var dropSql = "IF OBJECT_ID('" + viewName + "', 'P') IS NOT NULL DROP PROCEDURE " + viewName;
          SqlCommand command = new SqlCommand(dropSql, connection);
          command.Connection.Open();
          command.ExecuteNonQuery();

          command = new SqlCommand(viewSql, connection);
          command.ExecuteNonQuery();
        }
      }

      #endregion

    }
  }

}
