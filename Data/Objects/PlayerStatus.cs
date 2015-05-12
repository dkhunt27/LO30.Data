using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class PlayerStatus
  {
    [Key, Column(Order = 0)]
    public int PlayerStatusId { get; set; }

    [Required]
    public int PlayerId { get; set; }

    [Required]
    public int PlayerStatusTypeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    
    [Required]
    public bool Archive { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }

    [ForeignKey("PlayerStatusTypeId")]
    public virtual PlayerStatusType PlayerStatusType { get; set; }
  }
}