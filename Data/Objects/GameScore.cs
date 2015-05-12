using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class GameScore
  {
    [Required, Key, Column(Order = 1), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2),  ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, Key, Column(Order = 3)]
    public int Period { get; set; }

    [Required]
    public int Score { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    public virtual Game Game { get; set; }
    public virtual Team Team { get; set; }
    public virtual Season Season { get; set; }

    public GameScore()
    {
    }

    public GameScore(int gid, int sid, int tid, int per, int score)
    {
      this.GameId = gid;
      this.TeamId = tid;
      this.SeasonId = sid;

      this.Period = per;
      this.Score = score;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("gid: {0}, tid: {1}, per: {2}, sid: {3}",
                            this.GameId,
                            this.TeamId,
                            this.Period,
                            this.SeasonId);

      if (this.Period < 0 && this.Period > 4)
      {
        throw new ArgumentException("Period must be between 0 and 4 for:" + locationKey, "Period");
      }

      if (this.Score < 0)
      {
        throw new ArgumentException("Score must be a positive number for:" + locationKey, "Score");
      }
    }
  }
}