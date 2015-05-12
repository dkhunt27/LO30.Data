using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class ScoreSheetEntryPenalty
  {
    [Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int ScoreSheetEntryPenaltyId { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    public int Period { get; set; }

    [Required]
    public bool HomeTeam { get; set; }

    [Required]
    public int Player { get; set; }

    [Required, MaxLength(3)]
    public string PenaltyCode { get; set; }

    [Required, MaxLength(5)]
    public string TimeRemaining { get; set; }
    
    [Required]
    public int PenaltyMinutes { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
  }
}