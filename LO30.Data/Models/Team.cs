using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class Team
  {
    [Required]
    public int SeasonId { get; set; }

    [Key, Column(Order = 2), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int TeamId { get; set; }
    
    [Required, MaxLength(5)]
    public string TeamCode { get; set; }

    [Required, MaxLength(15)]
    public string TeamNameShort { get; set; }

    [Required, MaxLength(35)]
    public string TeamNameLong { get; set; }

    public int? CoachId { get; set; }

    public int? SponsorId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("CoachId")]
    public virtual Player Coach { get; set; }

    [ForeignKey("SponsorId")]
    public virtual Player Sponsor { get; set; }

    public Team()
    {
    }

    public Team(int sid, int tid, string tc, string tns, string tnl)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.TeamCode = tc;
      this.TeamNameShort = tns;
      this.TeamNameLong = tnl;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, tid: {1}",
                            this.SeasonId,
                            this.TeamId);
    }
  }
}