using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class ForWebGoalieStat
  {
    [Required, Key, Column(Order = 1)]
    public int SID { get; set; }

    [Required, Key, Column(Order = 2)]
    public int TID { get; set; }

    [Required, Key, Column(Order = 3)]
    public int PID { get; set; }

    [Required, Key, Column(Order = 4)]
    public bool PFS { get; set; }

    [Required, Key, Column(Order = 5), MaxLength(1)]
    public string Sub { get; set; }

    [Required, MaxLength(50)]
    public string Player { get; set; }

    [Required, MaxLength(35)]
    public string Team { get; set; }

    [Required]
    public int GP { get; set; }

    [Required]
    public int GA { get; set; }

    [Required]
    public double GAA { get; set; }

    [Required]
    public int SO { get; set; }

    [Required]
    public int W { get; set; }
  }
}