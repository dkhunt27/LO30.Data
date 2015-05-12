using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class ScoreSheetEntry
  {
    [Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int ScoreSheetEntryId { get; set; }

    [Required]
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

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
  }
}