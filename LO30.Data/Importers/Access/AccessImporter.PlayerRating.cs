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
    public ImportStat ImportPlayerRatings()
    {
      string table = "PlayerRatings";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.PlayerRatings.Count() == 0)
      {
        _logger.Write("Importing " + table);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "PlayerRatings.json");
        int count = parsedJson.Count;
        int countSaveOrUpdated = 0;

        _logger.Write("Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("Access records processed:" + d); }
          var json = parsedJson[d];

          string[] ratingParts = new string[0];

          if (json["PLAYER_RATING"] != null)
          {
            string rating = json["PLAYER_RATING"];
            ratingParts = rating.Split('.');
          }

          int ratingPrimary = -1;
          int ratingSecondary = -1;
          if (ratingParts.Length > 0)
          {
            ratingPrimary = Convert.ToInt32(ratingParts[0]);
            ratingSecondary = 0;
            if (ratingParts.Length > 1)
            {
              ratingSecondary = Convert.ToInt32(ratingParts[1]);
            }
          }

          int line = 0;
          if (json["PLAYER_LINE"] != null)
          {
            line = json["PLAYER_LINE"];
          }

          int seasonId = json["SEASON_ID"];
          int playerId = json["PLAYER_ID"];

          if (playerId == 545 || playerId == 512 || playerId == 426 || playerId == 432 || playerId == 381 || playerId == 282)
          {
            // skip these players...they do not exist in the players table
          }
          else
          {

            var season = _lo30ContextService.FindSeason(seasonId);
            var playerDraft = _lo30ContextService.FindPlayerDraft(seasonId, playerId);

            // default the players rating to the start/end of the season
            var playerRating = new PlayerRating(
                                      sid: seasonId,
                                      pid: playerId,
                                      symd: season.StartYYYYMMDD,
                                      eymd: season.EndYYYYMMDD,
                                      rp: ratingPrimary,
                                      rs: ratingSecondary,
                                      line: line
                                      );


            countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdatePlayerRating(playerRating);
          }
        }
        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.PlayerRatings.Count());

        // add missing players with default rating (0.0)

        //var players = _context.Players.ToList();

        //foreach (var player in players)
        //{
        //  var found = _context.PlayerRatings.Where(x => x.PlayerId == player.PlayerId).ToList();

        //  if (found == null || found.Count == 0)
        //  {
        //    // TODO, remove hard coding
        //    var season = _lo30ContextService.FindSeason(54);

        //    // default the players rating to the start/end of the season
        //    var playerRating = new PlayerRating(
        //                              sid: season.SeasonId,
        //                              pid: player.PlayerId,
        //                              symd: season.StartYYYYMMDD,
        //                              eymd: season.EndYYYYMMDD,
        //                              rp: 0,
        //                              rs: 0,
        //                              line: 0
        //                              );

        //    countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdatePlayerRating(playerRating);
        //  }
        //}
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