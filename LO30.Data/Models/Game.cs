using LO30.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class Game
  {
    [Required, Key, Column(Order = 1), DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    public int GameId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    [Required]
    public bool Playoffs { get; set; }

    [Required]
    public DateTime GameDateTime { get; set; }

    [Required]
    public int GameYYYYMMDD { get; set; }

    [Required, MaxLength(15)]
    public string Location { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }
    
    public Game()
    {
    }

    public Game(int sid, int gid, bool pfs, DateTime time, string loc)
    {
      this.SeasonId = sid;
      this.GameId = gid;
      this.Playoffs = pfs;

      this.GameDateTime = time;
      this.GameYYYYMMDD = ConvertDateTimeIntoYYYYMMDD(time, ifNullReturnMax: false);
      this.Location = loc;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, gid: {1}",
                            this.SeasonId,
                            this.GameId);
    }

    public int ConvertDateTimeIntoYYYYMMDD(DateTime? toConvert, bool ifNullReturnMax)
    {
      var lo30DataTimeService = new TimeService();
      return lo30DataTimeService.ConvertDateTimeIntoYYYYMMDD(toConvert, ifNullReturnMax);
    }
  }
}