using LO30.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class ProcessingResultGame
  {
    [Required, Key, ]
    public int GameId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    [Required]
    public bool Playoffs { get; set; }

    [Required]
    public bool GameTeams { get; set; }

    [Required]
    public bool ScoreSheetEntrySubs { get; set; }

    [Required]
    public bool ScoreSheetEntryProcessedSubs { get; set; }

    [Required]
    public bool GameRosters { get; set; }

    [Required]
    public bool ScoreSheetEntryGoals { get; set; }

    [Required]
    public bool ScoreSheetEntryProcessedGoals { get; set; }

    [Required]
    public bool ScoreSheetEntryPenalties { get; set; }

    [Required]
    public bool ScoreSheetEntryProcessedPenalties { get; set; }


    // virtual, foreign keys
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
    
 
  }
}