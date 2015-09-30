using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GoalieStatTeam
  {
    [Required, Key, Column(Order = 1), ForeignKey("Player")]
    public int PlayerId { get; set; }

    [Required, Key, Column(Order = 2), ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, Key, Column(Order = 3)]
    public bool Playoffs { get; set; }

    [Required, Key, Column(Order = 4)]
    public bool Sub { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    [Required]
    public int Games { get; set; }

    [Required]
    public int GoalsAgainst { get; set; }

    [Required]
    public double GoalsAgainstAverage
    { 
      get
      {
        return (double)GoalsAgainst / (double)Games;
      }
    }

    [Required]
    public int Shutouts { get; set; }

    [Required]
    public int Wins { get; set; }

    [Required]
    public DateTime UpdatedOn { get; set; }

    // virtual, foreign keys
    public virtual Season Season { get; set; }
    public virtual Team Team { get; set; }
    public virtual Player Player { get; set; }

    public GoalieStatTeam()
    {
    }

    public GoalieStatTeam(int pid, int tid, bool pfs, int sid, bool sub, int games, int ga, int so, int w)
    {
      this.PlayerId = pid;
      this.TeamId = tid;
      this.Playoffs = pfs;

      this.SeasonId = sid;
      this.Sub = sub;

      this.Games = games;

      this.GoalsAgainst = ga;
      this.Shutouts = so;
      this.Wins = w;

      this.UpdatedOn = DateTime.Now;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("pid: {0}, tid: {1}, sid: {2}, sub: {3}",
                                      this.PlayerId,
                                      this.TeamId,
                                      this.SeasonId,
                                      this.Sub);

      if (this.Shutouts < 0)
      {
        throw new ArgumentException("Shutouts cannot be a negative number for:" + locationKey, "Shutouts");
      }

      if (this.Wins < 0)
      {
        throw new ArgumentException("Wins cannot be a negative number for:" + locationKey, "Wins");
      }

      if (this.Wins > this.Games)
      {
        throw new ArgumentException("Wins must be less than or equal to Games for:" + locationKey, "Wins");
      }

      if (this.Shutouts > this.Games)
      {
        throw new ArgumentException("Shutouts must be less than or equal to Games for:" + locationKey, "Wins");
      }
    }
  }
}