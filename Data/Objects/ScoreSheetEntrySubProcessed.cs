using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class ScoreSheetEntrySubProcessed
  {
    [Required, Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int ScoreSheetEntrySubId { get; set; }

    [Required, Index("PK2", 1, IsUnique = true), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true), ForeignKey("SubPlayer")]
    public int SubPlayerId { get; set; }

    [Required]
    public bool HomeTeam { get; set; }

    [Required, ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, Index("PK2", 3, IsUnique = true), ForeignKey("SubbingForPlayer")]
    public int SubbingForPlayerId { get; set; }

    [Required]
    public string JerseyNumber { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    [Required]
    public DateTime UpdatedOn { get; set; }

    public virtual Game Game { get; set; }
    public virtual Team Team { get; set; }
    public virtual Player SubPlayer { get; set; }
    public virtual Player SubbingForPlayer { get; set; }
    public virtual Season Season { get; set; }

    public ScoreSheetEntrySubProcessed()
    {
    }

    public ScoreSheetEntrySubProcessed(int ssesid, int gid, bool ht, int tid, int sid, string jer, int spid, int sfpid, DateTime upd)
    {
      this.ScoreSheetEntrySubId = ssesid;

      this.GameId = gid;
      this.HomeTeam = ht;
      this.TeamId = tid;
      this.SeasonId = sid;
      this.JerseyNumber = jer;
      this.SubPlayerId = spid;
      this.SubbingForPlayerId = sfpid;

      this.UpdatedOn = upd;

      Validate();
    }


    private void Validate()
    {
      var locationKey = string.Format("ssesid: {0}, gid: {1}",
                            this.ScoreSheetEntrySubId,
                            this.GameId);
    }
  }
}