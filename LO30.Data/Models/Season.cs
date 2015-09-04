using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class Season
  {
    [Key, Column(Order = 1), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int SeasonId { get; set; }

    [Required, MaxLength(12)]
    public string SeasonName { get; set; }

    [Required]
    public bool IsCurrentSeason { get; set; }

    [Required]
    public int StartYYYYMMDD { get; set; }

    [Required]
    public int EndYYYYMMDD { get; set; }

    public Season()
    {
    }

    public Season(int sid, string sn, bool ics, int stymd, int endymd)
    {
      this.SeasonId = sid;
      this.SeasonName = sn;
      this.IsCurrentSeason = ics;
      this.StartYYYYMMDD = stymd;
      this.EndYYYYMMDD = endymd;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}",
                            this.SeasonId);
    }
  }
}