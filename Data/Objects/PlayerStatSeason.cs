using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class PlayerStatSeason
  {
    public PlayerStatSeason()
    {
      this.Games = 0;
      this.Goals = 0;
      this.Assists = 0;
      this.Points = 0;
      this.PenaltyMinutes = 0;
      this.PowerPlayGoals = 0;
      this.ShortHandedGoals = 0;
      this.GameWinningGoals = 0;

      Validate();
    }

    public PlayerStatSeason(int pid, int sid, int tid, bool pfs, int games, int g, int a, int p, int ppg, int shg, int gwg, int pim)
    {
      this.PlayerId = pid;
      this.SeasonId = sid;
      this.TeamId = tid;
      this.Playoffs = pfs;

      this.Games = games;

      this.Goals = g;
      this.Assists = a;
      this.Points = p;

      this.PowerPlayGoals = ppg;
      this.ShortHandedGoals = shg;
      this.GameWinningGoals = gwg;

      this.PenaltyMinutes = pim;

      Validate();
    }

    [Required, Key, Column(Order = 1), ForeignKey("Player")]
    public int PlayerId { get; set; }

    [Required, Key, Column(Order = 2), ForeignKey("Season")]
    public int SeasonId { get; set; }

    [Required, Key, Column(Order = 3), ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, Key, Column(Order = 4)]
    public bool Playoffs { get; set; }

    [Required]
    public int Games { get; set; }

    [Required]
    public int Goals { get; set; }

    [Required]
    public int Assists { get; set; }

    [Required]
    public int Points { get; set; }

    [Required]
    public int PenaltyMinutes { get; set; }

    [Required]
    public int PowerPlayGoals { get; set; }

    [Required]
    public int ShortHandedGoals { get; set; }

    [Required]
    public int GameWinningGoals { get; set; }

    public virtual Player Player { get; set; }
    public virtual Season Season { get; set; }
    public virtual Team Team { get; set; }

    private void Validate()
    {
      var locationKey = string.Format("pid: {0}, sid: {1}, tId: {2}, pfs: {3}",
                                  this.PlayerId,
                                  this.SeasonId,
                                  this.TeamId,
                                  this.Playoffs);

      if (this.Points != this.Goals + this.Assists)
      {
        throw new ArgumentException("Points must equal Goals + Assists for:" + locationKey, "Points");
      }

      if (this.PowerPlayGoals > this.Goals)
      {
        throw new ArgumentException("PowerPlayGoals must be less than or equal to Goals for:" + locationKey, "PowerPlayGoals");
      }

      if (this.ShortHandedGoals > this.Goals)
      {
        throw new ArgumentException("ShortHandedGoals must be less than or equal to Goals for:" + locationKey, "ShortHandedGoals");
      }

      if (this.GameWinningGoals > this.Goals)
      {
        throw new ArgumentException("GameWinningGoals must be less than or equal to Goals for:" + locationKey, "GameWinningGoals");
      }

      if (this.GameWinningGoals > this.Games)
      {
        throw new ArgumentException("GameWinningGoals must be less than or equal to Games for:" + locationKey, "GameWinningGoals");
      }
    }
  }
}