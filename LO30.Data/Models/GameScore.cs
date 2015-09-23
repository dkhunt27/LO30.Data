using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GameScore
  {
    [Required, Key, Column(Order = 1)]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2)]
    public int TeamId { get; set; }

    [Required, Key, Column(Order = 3)]
    public int Period { get; set; }

    [Required]
    public int Score { get; set; }

    [Required]
    public int SeasonId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }

    public GameScore()
    {
    }

    public GameScore(int sid, int tid, int gid, int per, int score)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.GameId = gid;

      this.Period = per;
      this.Score = score;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, tid: {1}, gid: {2}, per: {3}",
                            this.SeasonId,
                            this.TeamId,
                            this.GameId,
                            this.Period);

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