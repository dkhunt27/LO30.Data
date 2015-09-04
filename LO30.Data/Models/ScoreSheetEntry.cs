using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class ScoreSheetEntry
  {
    [Key, Column(Order = 1), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int ScoreSheetEntryId { get; set; }

    [Required, ForeignKey("Game")]
    public int GameId { get; set; }

    [Required]
    public int Period { get; set; }

    [Required]
    public bool HomeTeam { get; set; }

    [Required]
    public int Goal { get; set; }

    public int? Assist1 { get; set; }

    public int? Assist2 { get; set; }

    public int? Assist3 { get; set; }

    [Required, MaxLength(5)]
    public string TimeRemaining { get; set; }
    
    [MaxLength(2)]
    public string ShortHandedPowerPlay { get; set; }

    public DateTime UpdatedOn { get; set; }

    // virtual, foreign keys
    public virtual Game Game { get; set; }
  }
}