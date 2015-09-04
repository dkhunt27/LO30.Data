using LO30.Data.Contexts;
using LO30.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LO30.Data.Services
{
  public partial class DataAccessService
  {
    /*public List<ForWebPlayerStat> GetPlayerStatsForWeb(int seasonId, bool playoffs)
    {
      return _ctx.ForWebPlayerStats.Where(x => x.SID == seasonId && x.PFS == playoffs).ToList();
    }

    public DateTime GetPlayerStatsForWebDataGoodThru(int seasonId)
    {
      var maxGameData = _ctx.PlayerStatGames
              .GroupBy(x => new { x.SeasonId, x.Playoffs })
              .Select(grp => new
              {
                SeasonId = grp.Key.SeasonId,
                Playoffs = grp.Key.Playoffs,
                GameId = grp.Max(x => x.GameId),
                GameDateTime = grp.Max(x => x.Game.GameDateTime)
              })
              .Where(x => x.SeasonId == seasonId)
              .OrderByDescending(x => x.GameDateTime)
              .ToList();

      var gameDateTime = maxGameData.FirstOrDefault().GameDateTime;

      return gameDateTime;
    }*/
  }
}
