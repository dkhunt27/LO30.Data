using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class TeamStanding
  {
    [Key, Column(Order = 1)]
    public int TeamId { get; set; }

    [Key, Column(Order = 2)]
    public bool Playoffs { get; set; }

    [Required]
    public int SeasonId { get; set; }

    [Required]
    public int DivisionId { get; set; }

    [Required]
    public int Rank { get; set; }

    [Required]
    public int Games { get; set; }

    [Required]
    public int Wins { get; set; }

    [Required]
    public int Losses { get; set; }

    [Required]
    public int Ties { get; set; }

    [Required]
    public int Points { get; set; }

    [Required]
    public int GoalsFor { get; set; }

    [Required]
    public int GoalsAgainst { get; set; }

    [Required]
    public int PenaltyMinutes { get; set; }

    [Required]
    public int Subs { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("DivisionId")]
    public virtual Division Division { get; set; }

    public TeamStanding()
    {
      this.Games = 0;
      this.Wins = 0;
      this.Losses = 0;
      this.Ties = 0;
      this.Points = 0;
      this.GoalsFor = 0;
      this.GoalsAgainst = 0;
      this.PenaltyMinutes = 0;
      this.Subs = 0;
    }
  }
}