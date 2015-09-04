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
  class AccessTableList
  {
    public string ConnString { get; set; }
    public string QueryBegin { get; set; }
    public string QueryEnd { get; set; }
    public string TableName { get; set; }
    public string FileName { get; set; }
  }

  public class AccessDatabaseService
  {
    private string _folderPath;
    private string _connString;
    private string _connStringSSE;

    public AccessDatabaseService()
    {
      _connString = System.Configuration.ConfigurationManager.ConnectionStrings["LO30AccessDB"].ConnectionString;
      _connStringSSE = System.Configuration.ConfigurationManager.ConnectionStrings["LO30AccessDBSSE"].ConnectionString;
      _folderPath = @"C:\git\LO30\LO30\App_Data\Access\";
    }

    public void SaveObjToJsonFile(dynamic obj, string destPath)
    {
      var output = JsonConvert.SerializeObject(obj, Formatting.Indented);

      StringBuilder sb = new StringBuilder();
      sb.Append(output);

      using (StreamWriter outfile = new StreamWriter(destPath))
      {
        outfile.Write(sb.ToString());
      }
    }

    public dynamic ParseObjectFromJsonFile(string srcPath)
    {
      string contents = File.ReadAllText(srcPath);
      dynamic parsedJson = JsonConvert.DeserializeObject(contents);
      return parsedJson;
    }

    public void ProcessAccessTableToJsonFile(string connString, string queryBegin, string queryEnd, string table, string file)
    {
      //var results = new ProcessingResult();

      Debug.Print("ProcessAccessTableToJsonFile: Processing " + table);
      var last = DateTime.Now;

      var sql = queryBegin + " " + table + " " + queryEnd;
      var dsView = new DataSet();
      var adp = new OleDbDataAdapter(sql, connString);
      adp.Fill(dsView, "AccessData");
      adp.Dispose();
      var tbl = dsView.Tables["AccessData"];

      //result.toProcess = tbl.Rows.Count;

      SaveObjToJsonFile(tbl, _folderPath + file + ".json");

      //result.modified = tbl.Rows.Count;

      Debug.Print("ProcessAccessTableToJsonFile: Processed " + table);
      var diffFromLast = DateTime.Now - last;
      Debug.Print("TimeToProcess: " + diffFromLast.ToString());

      return;
    }

    public void SaveTablesToJson()
    {
      //var results = new ProcessingResult();
      //results.toProcess = 0;
      //results.modified = 0;

      DateTime first = DateTime.Now;
      DateTime last = DateTime.Now;
      TimeSpan diffFromFirst = new TimeSpan();

      var connString = System.Configuration.ConfigurationManager.ConnectionStrings["LO30AccessDB"].ConnectionString;

      List<AccessTableList> accessTables = new List<AccessTableList>()
      {
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, GAME_ID", TableName="GAME", FileName="Games"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, GAME_ID", TableName="GAME_ROSTER", FileName="GameRosters"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, GAME_ID, PERIOD, TIME_REMAINING DESC", TableName="PENALTY_DETAIL", FileName="PenaltyDetails"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT PLAYER_ID, PLAYER_FIRST_NAME, PLAYER_LAST_NAME, PLAYER_SUFFIX, PLAYER_POSITION, SHOOTS FROM", QueryEnd="ORDER BY PLAYER_ID", TableName="PLAYER", FileName="Players"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, PLAYER_ID", TableName="PLAYER_RATING", FileName="PlayerRatings"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY PLAYER_ID", TableName="PLAYER_STATUS", FileName="PlayerStatuses"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY PENALTY_ID", TableName="REF_PENALTY", FileName="Penalties"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SEASON_ID", TableName="REF_SEASON", FileName="Seasons"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY STATUS_ID", TableName="REF_STATUS", FileName="Statuses"},
        new AccessTableList(){ConnString=_connStringSSE, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SCORE_SHEET_ENTRY_ID", TableName="SCORE_SHEET_ENTRY", FileName="ScoreSheetEntries"},
        new AccessTableList(){ConnString=_connStringSSE, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SCORE_SHEET_ENTRY_PENALTY_ID", TableName="SCORE_SHEET_ENTRY_PENALTY", FileName="ScoreSheetEntryPenalties"},
        new AccessTableList(){ConnString=_connStringSSE, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SCORE_SHEET_ENTRY_SUB_ID", TableName="SCORE_SHEET_ENTRY_SUB", FileName="ScoreSheetEntrySubs"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, GAME_ID, PERIOD, TIME_REMAINING DESC", TableName="SCORING_DETAIL", FileName="ScoringDetails"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SEASON_ID, TEAM_ID", TableName="TEAM", FileName="Teams"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="WHERE SEASON_ID=54 ORDER BY SEASON_ID, TEAM_ID", TableName="TEAM_ROSTER", FileName="TeamRosters"},

        // addtl tables
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SEASON_ID, PLAYER_ID", TableName="FACT_PLAYER_STATS", FileName="FactPlayerStats"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SEASON_ID, PLAYER_ID", TableName="FACT_SUB_PLAYER_STATS", FileName="FactSubPlayerStats"},
        new AccessTableList(){ConnString=_connString, QueryBegin="SELECT * FROM", QueryEnd="ORDER BY SEASON_ID, TEAM_ID", TableName="FACT_TEAM_STATS", FileName="FactTeamStats"}
      };

      foreach (var table in accessTables)
      {
        ProcessAccessTableToJsonFile(table.ConnString, table.QueryBegin, table.QueryEnd, table.TableName, table.FileName);
        //var result = ProcessAccessTableToJsonFile(table.ConnString, table.QueryBegin, table.QueryEnd, table.TableName, table.FileName);

        //results.error = result.error;
       // results.toProcess += result.toProcess;
       // results.modified += result.modified;

        //if (!string.IsNullOrWhiteSpace(result.error))
        //{
        //  break;
        //}
      }

      diffFromFirst = DateTime.Now - first;
      Debug.Print("Total TimeToProcess: " + diffFromFirst.ToString());
      //results.time = diffFromFirst.ToString();

      return;
    }
  }
}
