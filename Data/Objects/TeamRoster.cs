using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class TeamRoster
  {
    [Key, Column(Order = 0)]
    public int SeasonId { get; set; }

    [Key, Column(Order = 1)]
    public int TeamId { get; set; }

    [Key, Column(Order = 2)]
    public int PlayerId { get; set; }

    [Key, Column(Order = 3)]
    public int SeasonTypeId { get; set; }

    public int? PlayerNumber { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }

    [ForeignKey("SeasonTypeId")]
    public virtual SeasonType SeasonType { get; set; }
  }
}