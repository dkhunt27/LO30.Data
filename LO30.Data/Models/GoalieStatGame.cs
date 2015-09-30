using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GoalieStatGame
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
    public int GoalsAgainst { get; set; }

    [Required]
    public int Shutouts { get; set; }

    [Required]
    public int Wins { get; set; }

    [Required]
    public DateTime UpdatedOn { get; set; }

    // virtual, foreign keys
    public virtual Season Season { get; set; }
    public virtual Team Team { get; set; }
    public virtual Game Game { get; set; }
    public virtual Player Player { get; set; }

    public GoalieStatGame()
    {
    }

    public GoalieStatGame(int pid, int gid, int tid, bool pfs, int sid, bool sub, int ga, int so, int w)
    {
      this.PlayerId = pid;
      this.GameId = gid;
      this.TeamId = tid;
      this.Playoffs = pfs;
      this.SeasonId = sid;
      this.Sub = sub;

      this.GoalsAgainst = ga;
      this.Shutouts = so;
      this.Wins = w;

      this.UpdatedOn = DateTime.Now;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("pid: {0}, gid: {1}, tid: {2}, pfs: {3}, sid: {4}, sub: {5}",
                                      this.PlayerId,
                                      this.GameId,
                                      this.TeamId,
                                      this.Playoffs,
                                      this.SeasonId,
                                      this.Sub);

      if (this.Shutouts < 0 || this.Shutouts > 1)
      {
        throw new ArgumentException("Shutouts can only be 0 or 1 for:" + locationKey, "Shutouts");
      }

      if (this.Wins < 0 || this.Wins > 1)
      {
        throw new ArgumentException("Wins can only be 0 or 1 for:" + locationKey, "Wins");
      }
    }
  }
}