using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class ScoreSheetEntryProcessed
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
    public int GoalPlayerId { get; set; }

    public int? Assist1PlayerId { get; set; }

    public int? Assist2PlayerId { get; set; }

    public int? Assist3PlayerId { get; set; }

    [Required, MaxLength(5)]
    public string TimeRemaining { get; set; }

    [Required]
    public bool ShortHandedGoal { get; set; }

    [Required]
    public bool PowerPlayGoal { get; set; }

    [Required]
    public bool GameWinningGoal { get; set; }

    [Required]
    public int SeasonId { get; set; }

    [Required]
    public int TeamId { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }

    [ForeignKey("GoalPlayerId")]
    public virtual Player GoalPlayer { get; set; }

    [ForeignKey("Assist1PlayerId")]
    public virtual Player Assist1Player { get; set; }

    [ForeignKey("Assist2PlayerId")]
    public virtual Player Assist2Player { get; set; }

    [ForeignKey("Assist3PlayerId")]
    public virtual Player Assist3Player { get; set; }

    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
  }
}