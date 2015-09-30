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
    public ImportStat ImportTeamRosters()
    {
      string table = "TeamRosters";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.TeamRosters.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "TeamRosters.json");
        int count = parsedJson.Count;

        _logger.Write("SaveOrUpdateTeamRosters: Access records to process:" + count);

        int countSaveOrUpdated = 0;
        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("SaveOrUpdateTeamRosters: Access records processed:" + d); }
          var json = parsedJson[d];

          int seasonId = json["SEASON_ID"];
          bool playoff = json["PLAYOFF_SEASON_IND"];

          int teamId = json["TEAM_ID"];
          int playerId = json["PLAYER_ID"];

          int playerNumber = -1;
          if (json["PLAYER_NUMBER"] != null)
          {
            playerNumber = json["PLAYER_NUMBER"];
          }

          // based on the draft spot, determine team roster line and position
          var team = _lo30ContextService.FindTeam(teamId);
          var season = _lo30ContextService.FindSeason(seasonId);

          PlayerDraft playerDraft;
          if (team.SeasonId == 54 && playoff == false && playerId == 66)
          {
            // HACK FIX for Bill Hamilton rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: 4, ord: -1, pos: "F", line: 1, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == false && playerId == 662)
          {
            // HACK FIX for Matt Spease rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: 10, ord: -1, pos: "F", line: 2, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 674)
          {
            // HACK FIX for Bob Hickson rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "D", line: 1, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 681)
          {
            // HACK FIX for Geoff Cutsy rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "F", line: 1, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 757)
          {
            // HACK FIX for Gary Zielke rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "D", line: 1, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 758)
          {
            // HACK FIX for Paul Fretter rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "F", line: 3, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 721)
          {
            // HACK FIX for Tom Small rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "D", line: 2, spcl: false);
          }
          else if (team.SeasonId == 54 && playoff == true && playerId == 581)
          {
            // HACK FIX for Kyle Krupsky rostered sub
            playerDraft = new PlayerDraft(sid: team.SeasonId, pid: playerId, rnd: -1, ord: -1, pos: "F", line: 2, spcl: false);
          }
          else
          {
            playerDraft = _lo30ContextService.FindPlayerDraft(team.SeasonId, playerId);
          }

          PlayerRating playerRating;
          if (team.SeasonId == 54 && playoff == false && playerId == 66)
          {
            // HACK FIX for Bill Hamilton rostered sub
            playerRating = new PlayerRating(sid: team.SeasonId, pid: playerId, symd: 20140904, eymd: 20141031, rp: 2, rs: 1, line: 1, pos: "F");
          }
          else if (team.SeasonId == 54 && playoff == false && playerId == 662)
          {
            // HACK FIX for Matt Spease rostered sub
            playerRating = new PlayerRating(sid: team.SeasonId, pid: playerId, symd: 20140904, eymd: 20141031, rp: 6, rs: 2, line: 2, pos: "F");
          }
          else
          {
            playerRating = _lo30ContextService.FindPlayerRatingWithYYYYMMDD(playerId, playerDraft.Position, team.SeasonId, season.StartYYYYMMDD);
          }

          // default the team roster to the start/end of the season
          TeamRoster teamRoster;
          if (team.SeasonId == 54 && playoff == false && playerId == 710)
          {
            // HACK FIX for Bill Hamilton rostered sub (Howard)
            // add billy
            teamRoster = new TeamRoster(sid: team.SeasonId, tid: team.TeamId, pid: 66, symd: 20140904, eymd: 20141031, pos: "F", rp: 2, rs: 1, line: 1, pn: 15);
            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateTeamRoster(teamRoster);

            // add howard
            teamRoster = new TeamRoster(sid: team.SeasonId, tid: team.TeamId, pid: playerId, symd: 20141101, eymd: 20150118, pos: playerDraft.Position, rp: playerRating.RatingPrimary, rs: playerRating.RatingSecondary, line: playerDraft.Line, pn: playerNumber);
            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateTeamRoster(teamRoster);

          }
          else if (team.SeasonId == 54 && playoff == false && playerId == 708)
          {
            // HACK FIX for Matt Spease rostered sub (Vince)
            // add matt
            teamRoster = new TeamRoster(sid: team.SeasonId, tid: team.TeamId, pid: 662, symd: 20140904, eymd: 20141031, pos: "F", rp: 6, rs: 2, line: 2, pn: 17);
            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateTeamRoster(teamRoster);

            // add vince
            teamRoster = new TeamRoster(sid: team.SeasonId, tid: team.TeamId, pid: playerId, symd: 20141101, eymd: 20150118, pos: playerDraft.Position, rp: playerRating.RatingPrimary, rs: playerRating.RatingSecondary, line: playerDraft.Line, pn: playerNumber);
            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateTeamRoster(teamRoster);
          }
          else
          {
            #region else
            int startYYYYMMDD, endYYYYMMDD;

            if (
                  seasonId == 54 && playoff == true &&
                  (
                    playerId == 762 || playerId == 64 || playerId == 594 ||
                    playerId == 33 ||
                    playerId == 178 || playerId == 674 ||
                    playerId == 581 ||
                    playerId == 721 ||
                    playerId == 757 || playerId == 758
                  )
              )
            {
              // 762 Scott Ranta, 64 Mark Ranta, 594 BJ to LAB
              // 33 Todd Keller to Zas Ent
              // 178 Ken Grant, 674 Bob Hickson (708 Vince DeMassa...from regular season) to Bill Brown
              // 581 Kyle Krupsky (710 Howard Schoenfeldt...from regular season) to D&G
              // 721 Tom Small to Hunt's Ace
              // 757 Gary Zielke, 758 Paul Fretter to Glover
              // 686 Kris Medico, 681 Geoff Cutsy to DPKZ (Joe/Pete still on same team)
              startYYYYMMDD = 20150122;
              endYYYYMMDD = 20150322;
            }
            else if (
                seasonId == 54 && playoff == true &&
                (
                  playerId == 708 || playerId == 710
                )
            )
            {
              // (708 Vince DeMassa...from regular season) to Bill Brown
              // (710 Howard Schoenfeldt...from regular season) to D&G
              startYYYYMMDD = 20141101;
              endYYYYMMDD = 20150322;
            }
            else if (seasonId == 54 && playoff == false)
            {
              // if roster is before playoffs, we don't know if they will be traded
              // so default to end of regular season date...so if they get traded, don't have to 
              // go back and update the old/initial record
              startYYYYMMDD = 20140904;
              endYYYYMMDD = 20150118;
            }
            else if (seasonId == 54 && playoff == true)
            {
              // if they were not one of the guys who were affected by the trades,
              // change their end date to the end of the year
              startYYYYMMDD = 20140904;
              endYYYYMMDD = 20150322;
            }
            else
            {
              startYYYYMMDD = season.StartYYYYMMDD;
              endYYYYMMDD = season.EndYYYYMMDD;
            }

            teamRoster = new TeamRoster(sid: team.SeasonId, tid: team.TeamId, pid: playerId, symd: startYYYYMMDD, eymd: endYYYYMMDD, pos: playerDraft.Position, rp: playerRating.RatingPrimary, rs: playerRating.RatingSecondary, line: playerDraft.Line, pn: playerNumber);
            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdateTeamRoster(teamRoster);
            #endregion
          }
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.TeamRosters.Count());
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
  