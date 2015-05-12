using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class Season
  {
    [Key, Column(Order = 0), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int SeasonId { get; set; }

    [Required, MaxLength(12)]
    public string SeasonName { get; set; }

    [Required]
    public bool IsCurrentSeason { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
  }
}