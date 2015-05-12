using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class SeasonType
  {
    [Key, Column(Order = 0)]
    public int SeasonTypeId { get; set; }

    [Required, MaxLength(25)]
    public string SeasonTypeName { get; set; }
    
    [Required]
    public bool RegularSeason { get; set; }

    [Required]
    public bool PlayoffSeason { get; set; }
  }
}