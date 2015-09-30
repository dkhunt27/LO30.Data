using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GameRoster
  {
    private const int _gridDefault = 0;

    [Required, Key]
    public int GameRosterId { get; set; }

    [Required, Index("PK2", 1, IsUnique = true), Index("PK3", 1, IsUnique = true)]
    public int GameId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true), Index("PK3", 2, IsUnique = true)]
    public int TeamId { get; set; }

    [Required, Index("PK2", 3, IsUnique = true), MaxLength(3)]
    public string PlayerNumber { get; set; }

    [Required, Index("PK3", 3, IsUnique = true)]
    public int PlayerId { get; set; }

    [Required, MaxLength(1)]
    public string Position { get; set; }

    [Required]
    public int RatingPrimary { get; set; }

    [Required]
    public int RatingSecondary { get; set; }

    [Required]
    public int Line { get; set; }

    [Required]
    public bool Goalie { get; set; }

    [Required]
    public bool Sub { get; set; }

    public int? SubbingForPlayerId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }

    [ForeignKey("SubbingForPlayerId")]
    public virtual Player SubbingForPlayer { get; set; }

    public GameRoster()
    {
    }

    public GameRoster(int sid, int tid, int gid, int pid, string pn,  string pos, int rp, int rs, int line, bool g, bool sub, int? sfpid) :
      this(sid, tid, gid, _gridDefault, pid, pn, pos, rp, rs, line, g, sub, sfpid)
    {
    }

    public GameRoster(int sid, int tid, int gid, int grid, int pid, string pn, string pos, int rp, int rs, int line, bool g, bool sub, int? sfpid)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.GameId = gid;
      this.GameRosterId = grid;

      this.PlayerId = pid;
      this.PlayerNumber = pn;

      this.Position = pos;
      this.RatingPrimary = rp;
      this.RatingSecondary = rs;
      this.Line = line;
      this.Goalie = g;

      this.Sub = sub;
      this.SubbingForPlayerId = sfpid;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, tid: {1}, gid: {2}, grid: {3}, pn: {4}",
                            this.SeasonId,
                            this.TeamId,
                            this.GameId,
                            this.GameRosterId,
                            this.PlayerNumber);

      if (this.Sub == true && this.SubbingForPlayerId == null)
      {
        throw new ArgumentException("If Sub is true, SubbingForPlayerId must be populated for:" + locationKey, "SubbingForPlayerId");
      }

      if (this.Sub == false && this.SubbingForPlayerId != null)
      {
        throw new ArgumentException("If Sub is false, SubbingForPlayerId must not be populated for:" + locationKey, "SubbingForPlayerId");
      }

      if (this.Position != "G" && this.Position != "D" && this.Position != "F")
      {
        throw new ArgumentException("Position('" + this.Position + "') must be 'G', 'D', or 'F' for:" + locationKey, "Position");
      }

      if (this.Position == "G" && this.Goalie != true)
      {
        throw new ArgumentException("If Position = 'G', Goalie must be true:" + locationKey, "Goalie");
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
    }
  }
}