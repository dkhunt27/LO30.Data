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
  public class ScoreSheetEntrySub
  {
    [Required, Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int ScoreSheetEntrySubId { get; set; }

    [Required, ForeignKey("Game")]
    public int GameId { get; set; }

    [Required]
    public int SubPlayerId { get; set; }

    [Required]
    public bool HomeTeam { get; set; }

    [Required]
    public int SubbingForPlayerId { get; set; }

    [Required]
    public string JerseyNumber { get; set; }

    [Required]
    public DateTime UpdatedOn { get; set; }

    public virtual Game Game { get; set; }
  }
}