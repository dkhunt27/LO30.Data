using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class DataProcessing
  {
    [Required, Key]
    public int Id { get; set; }

    [Required, Index("PK2", 1, IsUnique = true)]
    public int Name { get; set; }

    [Required]
    public DateTime Event { get; set; }
  }
}