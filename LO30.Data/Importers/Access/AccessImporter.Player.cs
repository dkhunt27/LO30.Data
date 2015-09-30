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
    public ImportStat ImportPlayers()
    {
      string table = "Players";
      var iStat = new ImportStat(_logger, table);

      if (_seed && _context.Players.Count() == 0)
      {
        _logger.Write("Importing " + table);

        var player = new Player()
        {
          PlayerId = 0,
          FirstName = "Unknown",
          LastName = "Player",
          Suffix = null,
          PreferredPosition = "X",
          Shoots = "X",
          BirthDate = DateTime.Parse("1/1/1970"),
          Profession = null,
          WifesName = null
        };

        _lo30ContextService.SaveOrUpdatePlayer(player);

        dynamic parsedJson = _jsonFileService.ParseObjectFromJsonFile(_folderPath + "Players.json");
        int count = parsedJson.Count;
        int countSaveOrUpdated = 0;

        _logger.Write("SaveOrUpdatePlayers: Access records to process:" + count);

        for (var d = 0; d < parsedJson.Count; d++)
        {
          if (d % 100 == 0) { _logger.Write("SaveOrUpdatePlayers: Access records processed:" + d + ". Records saved or updated:" + countSaveOrUpdated); }
          var json = parsedJson[d];
          int playerId = json["PLAYER_ID"];

          string firstName = json["PLAYER_FIRST_NAME"];
          if (string.IsNullOrWhiteSpace(firstName))
          {
            firstName = "_";
          };

          string lastName = json["PLAYER_LAST_NAME"];
          if (string.IsNullOrWhiteSpace(lastName))
          {
            lastName = "_";
          };

          string position, positionMapped;
          position = json["PLAYER_POSITION"];

          if (string.IsNullOrWhiteSpace(position))
          {
            position = "X";
          }

          switch (position.ToLower())
          {
            case "f":
            case "forward":
              positionMapped = "F";
              break;
            case "d":
            case "defense":
              positionMapped = "D";
              break;
            case "g":
            case "goal":
            case "goalie":
              positionMapped = "G";
              break;
            default:
              positionMapped = "X";
              break;
          }

          string shoots, shootsMapped;
          shoots = json["SHOOTS"];
          if (string.IsNullOrWhiteSpace(shoots))
          {
            shoots = "X";
          }

          switch (shoots.ToLower())
          {
            case "l":
              shootsMapped = "L";
              break;
            case "r":
              shootsMapped = "R";
              break;
            default:
              shootsMapped = "X";
              break;
          }

          DateTime? birthDate = null;

          if (json["BIRTHDATE"] != null)
          {
            birthDate = json["BIRTHDATE"];
          }

          player = new Player()
          {
            PlayerId = playerId,
            FirstName = firstName,
            LastName = lastName,
            Suffix = json["PLAYER_SUFFIX"],
            PreferredPosition = positionMapped,
            Shoots = shootsMapped,
            BirthDate = birthDate,
            Profession = json["PROFESSION"],
            WifesName = json["WIFES_NAME"]
          };

          countSaveOrUpdated = countSaveOrUpdated + _lo30ContextService.SaveOrUpdatePlayer(player);
        }

        iStat.Imported();
        ContextSaveChanges();
        iStat.Saved(_context.Players.Count());
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