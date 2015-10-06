using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class ForWebGoodThru
  {
    [Required, Key, Column(Order = 1)]
    public int ID { get; set; }

    [Required, MaxLength(25)]
    public string GT { get; set; }
  }
}