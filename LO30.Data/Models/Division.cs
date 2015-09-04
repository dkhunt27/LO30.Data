using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class Division
  {
    [Required, Key, Column(Order = 1)]
    public int DivisionId { get; set; }

    [Required, MaxLength(50), Index("PK2", 1, IsUnique = true)]
    public string DivisionLongName { get; set; }

    [Required, MaxLength(15)]
    public string DivisionShortName { get; set; }

    public Division()
    {
    }

    public Division(int did, string dln, string dsn)
    {
      this.DivisionId = did;
      this.DivisionLongName = dln;
      this.DivisionShortName = dsn;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("did: {0}",
                            this.DivisionId);
    }
  }
}