using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GameTeam
  {
    [Required, Key, Column(Order = 1), Index("PK2", 1, IsUnique = true)]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2)]
    public int TeamId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true)]
    public bool HomeTeam { get; set; }

    [Required]
    public int OpponentTeamId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }

    [ForeignKey("OpponentTeamId")]
    public virtual Team OpponentTeam { get; set; }

    public GameTeam()
    {
    }

    public GameTeam(int sid, int gid, int tid, bool ht, int otid)
    {
      this.SeasonId = sid;
      this.GameId = gid;
      this.TeamId = tid;
      this.HomeTeam = ht;
      this.OpponentTeamId = otid;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, gid: {1}, tid: {2}, ht: {3}",
                            this.SeasonId,
                            this.GameId,
                            this.TeamId,
                            this.HomeTeam);

    }
  }
}