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
    [Key, Column(Order = 1), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int TeamId { get; set; }

    [Required]
    public int SeasonId { get; set; }
    
    [Required, MaxLength(5)]
    public string TeamCode { get; set; }

    [Required, MaxLength(15)]
    public string TeamNameShort { get; set; }

    [Required, MaxLength(35)]
    public string TeamNameLong { get; set; }

    [Required]
    public int DivisionId { get; set; }

    public int? CoachId { get; set; }

    public int? SponsorId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("CoachId")]
    public virtual Player Coach { get; set; }

    [ForeignKey("SponsorId")]
    public virtual Player Sponsor { get; set; }

    [ForeignKey("DivisionId")]
    public virtual Division Division { get; set; }

    public Team()
    {
    }

    public Team(int sid, int tid, string tc, string tns, string tnl, int did)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.TeamCode = tc;
      this.TeamNameShort = tns;
      this.TeamNameLong = tnl;
      this.DivisionId = did;

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