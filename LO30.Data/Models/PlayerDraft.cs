using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class PlayerDraft
  {
    [Required, Key, Column(Order = 1)]
    public int SeasonId { get; set; }

    [Required, Key, Column(Order = 2)]
    public int PlayerId { get; set; }

    [Required]
    public int Round { get; set; }

    [Required]
    public int Order { get; set; }

    [Required]
    public string Position { get; set; }

    [Required]
    public int Line { get; set; }

    [Required]
    public bool Special { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }

    public PlayerDraft()
    {
    }

    public PlayerDraft(int sid, int pid, int rnd, int ord, string pos, int line, bool spcl)
    {
      this.SeasonId = sid;
      this.PlayerId = pid;
      this.Round = rnd;
      this.Order = ord;
      this.Position = pos;
      this.Line = line;
      this.Special = spcl;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, pid: {1}",
                            this.SeasonId,
                            this.PlayerId);

      if (this.Position != "G" && this.Position != "D" && this.Position != "F" && this.Position != "X")
      {
        throw new ArgumentException("Position('" + this.Position + "') must be 'X', 'G', 'D', or 'F' for:" + locationKey, "Position");
      }

      if (this.Line < 0 || this.Line > 3)
      {
        throw new ArgumentException("Line(" + this.Line + ") must be between 0 and 3:" + locationKey, "Line");
      }
    }
  }
}