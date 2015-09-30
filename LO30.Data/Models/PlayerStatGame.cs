using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class PlayerStatGame
  {
    [Required, Key, Column(Order = 1), ForeignKey("Player")]
    public int PlayerId { get; set; }

    [Required, Key, Column(Order = 2), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required]
    public bool Playoffs { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    [Required]
    public bool Sub { get; set; }

    [Required]
    public int Line { get; set; }

    [Required, MaxLength(1)]
    public string Position { get; set; }

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

    [Required]
    public DateTime UpdatedOn { get; set; }

    // virtual, foreign keys
    public virtual Season Season { get; set; }
    public virtual Team Team { get; set; }
    public virtual Game Game { get; set; }
    public virtual Player Player { get; set; }

    public PlayerStatGame()
    {
      this.Goals = 0;
      this.Assists = 0;
      this.Points = 0;
      this.PenaltyMinutes = 0;
      this.PowerPlayGoals = 0;
      this.ShortHandedGoals = 0;
      this.GameWinningGoals = 0;

      Validate();
    }

    public PlayerStatGame(int pid, int gid, int tid, bool pfs, int sid, bool sub, int line, string pos, int g, int a, int p, int ppg, int shg, int gwg, int pim)
    {
      this.PlayerId = pid;
      this.GameId = gid;
      this.TeamId = tid;
      this.Playoffs = pfs;
      this.SeasonId = sid;

      this.Line = line;
      this.Position = pos;
      this.Sub = sub;

      this.Goals = g;
      this.Assists = a;
      this.Points = p;

      this.PowerPlayGoals = ppg;
      this.ShortHandedGoals = shg;
      this.GameWinningGoals = gwg;

      this.PenaltyMinutes = pim;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("pid: {0}, gid: {1}, tid: {2}, pfs: {3}, sid: {4}",
        this.PlayerId,
        this.GameId,
        this.TeamId,
        this.Playoffs,
        this.SeasonId);

      // make sure points is goals + assists
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

      if (this.GameWinningGoals > 1)
      {
        throw new ArgumentException("GameWinningGoals must be less than or equal to 1 for:" + locationKey, "GameWinningGoals");
      }
    }
  }
}