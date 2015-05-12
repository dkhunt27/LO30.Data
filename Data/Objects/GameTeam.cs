using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class GameTeam
  {
    [Required, Key, Column(Order = 1), Index("PK2", 1, IsUnique = true), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2), ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true)]
    public bool HomeTeam { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    public virtual Game Game { get; set; }
    public virtual Team Team { get; set; }
    public virtual Season Season { get; set; }

    public GameTeam()
    {
    }

    public GameTeam(int gid, int sid, int tid, bool ht)
    {
      this.GameId = gid;
      this.TeamId = tid;
      this.HomeTeam = ht;
      this.SeasonId = sid;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("gid: {0}, tid: {1}, ht: {2}, sid: {3}",
                            this.GameId,
                            this.TeamId,
                            this.HomeTeam,
                            this.SeasonId);

    }
  }
}