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
    public ImportStat ImportTeams()
    {
      string table = "Teams";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Teams.Count() == 0)
      {
        _logger.Write("Importing " + table);

        #region add position night teams
        var seasonIdPlaceholder = -1;
        var divisionIdPlaceholder = 1;
        var team = new Team(sid: seasonIdPlaceholder, tid: -1, tc: "1TH", tns: "1st Place", tnl: "1st Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -2, tc: "2TH", tns: "2nd Place", tnl: "2nd Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -3, tc: "3TH", tns: "3rd Place", tnl: "3rd Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -4, tc: "4TH", tns: "4th Place", tnl: "4th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -5, tc: "5TH", tns: "5th Place", tnl: "5th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -6, tc: "6TH", tns: "6th Place", tnl: "6th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -7, tc: "7TH", tns: "7th Place", tnl: "7th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -8, tc: "8TH", tns: "8th Place", tnl: "8th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -9, tc: "9TH", tns: "9th Place", tnl: "9th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -10, tc: "10TH", tns: "10th Place", tnl: "10th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -11, tc: "11TH", tns: "11th Place", tnl: "11th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -12, tc: "12TH", tns: "12th Place", tnl: "12th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -13, tc: "13TH", tns: "13th Place", tnl: "13th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -14, tc: "14TH", tns: "14th Place", tnl: "14th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -15, tc: "15TH", tns: "15th Place", tnl: "15th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        team = new Team(sid: seasonIdPlaceholder, tid: -16, tc: "16TH", tns: "16th Place", tnl: "16th Place Team Placeholder", did: divisionIdPlaceholder);
        _context.Teams.Add(team);
        #endregion

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Teams.json");
        int count = parsedJson.Count;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          string teamCode = json["TEAM_SHORT_NAME"].ToString();
          if (teamCode.Length > 5)
          {
            teamCode = teamCode.Substring(0, 5);
          }

          team = new Team(sid: Convert.ToInt32(json["SEASON_ID"]), tid: Convert.ToInt32(json["TEAM_ID"]), tc: teamCode, tns: json["TEAM_SHORT_NAME"].ToString(), tnl: json["TEAM_LONG_NAME"].ToString(), did: divisionIdPlaceholder);
          _context.Teams.Add(team);
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Teams.Count());
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