using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class PlayerRating
  {
    [Key, Column(Order = 0)]
    public int SeasonId { get; set; }
    
    [Key, Column(Order = 1)]
    public int PlayerId { get; set; }

    [Required]
    public int RatingPrimary { get; set; }

    [Required]
    public int RatingSecondary { get; set; }
    
    [Required]
    public int Line { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }
  }
}