using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class ScoreSheetEntryGoalType
  {
    [Required, Key]
    public int ScoreSheetEntryId { get; set; }

    [Required]
    public bool ShortHandedGoal { get; set; }

    [Required]
    public bool PowerPlayGoal { get; set; }

    [Required]
    public bool GameWinningGoal { get; set; }
  }
}