using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class TeamRoster
  {
    [Key, Column(Order = 1)]
    public int TeamId { get; set; }

    [Key, Column(Order = 2)]
    public int PlayerId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    [Required]
    public int StartYYYYMMDD { get; set; }

    [Required]
    public int EndYYYYMMDD { get; set; }

    [Required]
    public string Position { get; set; }

    [Required]
    public int RatingPrimary { get; set; }

    [Required]
    public int RatingSecondary { get; set; }

    [Required]
    public int Line { get; set; }

    [MaxLength(3)]
    public string PlayerNumber { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }

    public TeamRoster()
    {
    }

    public TeamRoster(int sid, int tid, int pid, int symd, int eymd, string pos, int rp, int rs, int line, int pn)
      : this(sid, tid, pid, symd, eymd, pos, rp, rs, line, pn.ToString())
    {
    }

    public TeamRoster(int sid, int tid, int pid, int symd, int eymd, string pos, int rp, int rs, int line, string pn)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.PlayerId = pid;
      this.StartYYYYMMDD = symd;
      this.EndYYYYMMDD = eymd;

      this.Position = pos;
      this.RatingPrimary = rp;
      this.RatingSecondary = rs;
      this.Line = line;
      this.PlayerNumber = pn;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, tid: {1}, pid: {2}, symd: {3}, eymd: {4}",
                            this.SeasonId,
                            this.TeamId,
                            this.PlayerId,
                            this.StartYYYYMMDD,
                            this.EndYYYYMMDD);

      if (this.Position != "G" && this.Position != "D" && this.Position != "F")
      {
        throw new ArgumentException("Position('" + this.Position + "') must be 'G', 'D', or 'F' for:" + locationKey, "Position");
      }

      if (this.Line < 1 || this.Line > 3)
      {
        throw new ArgumentException("Line(" + this.Line + ") must be between 1 and 3:" + locationKey, "Line");
      }

      if (this.RatingPrimary < 0 || this.RatingPrimary > 9)
      {
        throw new ArgumentException("RatingPrimary(" + this.RatingPrimary + ") must be between 0 and 9:" + locationKey, "RatingPrimary");
      }

      if (this.RatingSecondary < 0 || this.RatingSecondary > 8)
      {
        throw new ArgumentException("RatingSecondary(" + this.RatingSecondary + ") must be between 0 and 8:" + locationKey, "RatingSecondary");
      }

      int playerNumber = -1;
      if (!int.TryParse(this.PlayerNumber, out playerNumber))
      {
        throw new ArgumentException("PlayerNumber(" + this.PlayerNumber + ") must be a number:" + locationKey, "PlayerNumber");
      }

      if (playerNumber < -1 || playerNumber > 99)
      {
        throw new ArgumentException("PlayerNumber(" + this.PlayerNumber + ") must be between 0 and 99:" + locationKey, "PlayerNumber");
      }
    }
  }
}