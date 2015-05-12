using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class GameRoster
  {
    private const int _gridDefault = 0;
    private const int _rpDefault = 0;
    private const int _rsDefault = 0;

    [Required, Key]
    public int GameRosterId { get; set; }

    [Required, Index("PK2", 1, IsUnique = true), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true), ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, ForeignKey("Player")]
    public int PlayerId { get; set; }

    [Required, Index("PK2", 3, IsUnique = true), MaxLength(3)]
    public string PlayerNumber { get; set; }

    [Required, MaxLength(1)]
    public string Position { get; set; }

    // TODO, make the rating, line required
    //[Required]
    public int RatingPrimary { get; set; }

    //[Required]
    public int RatingSecondary { get; set; }

    //[Required]
    public int Line { get; set; }

    [Required]
    public bool Goalie { get; set; }

    [Required]
    public bool Sub { get; set; }

    [ForeignKey("SubbingForPlayer")]
    public int? SubbingForPlayerId { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    public virtual Game Game { get; set; }
    public virtual Team Team { get; set; }
    public virtual Player Player { get; set; }
    public virtual Player SubbingForPlayer { get; set; }
    public virtual Season Season { get; set; }

    // ctr0
    public GameRoster()
    {
    }

    // ctr1 (pn as int; no grid, rp, rs)
    public GameRoster(int gid, int sid, int tid, int pn, int line, string pos, bool g, int pid, bool sub, int? sfpid) :
      this(_gridDefault, gid, sid, tid, pn.ToString(), line, pos, g, pid, _rpDefault, _rsDefault, sub, sfpid)  // ctr6
    {
    }

    // ctr2 (pn as int; no rp, rs)
    public GameRoster(int grid, int gid, int sid, int tid, int pn, int line, string pos, bool g, int pid, bool sub, int? sfpid) :
      this(grid, gid, sid, tid, pn.ToString(), line, pos, g, pid, _rpDefault, _rsDefault, sub, sfpid)  // ctr6
    {
    }

    // ctr3 (pn as string; no grid, rp, rs)
    public GameRoster(int gid, int sid, int tid, string pn, int line, string pos, bool g, int pid, bool sub, int? sfpid) :
      this(_gridDefault, gid, sid, tid, pn, line, pos, g, pid, _rpDefault, _rsDefault, sub, sfpid)  // ctr6
    {
    }

    // ctr4  (pn as string; no rp, rs)
    public GameRoster(int grid, int gid, int sid, int tid, string pn, int line, string pos, bool g, int pid, bool sub, int? sfpid) :
      this(grid, gid, sid, tid, pn, line, pos, g, pid, _rpDefault, _rsDefault, sub, sfpid)  // ctr6
    {
    }

    // ctr5  (pn as string; no grid)
    public GameRoster(int gid, int sid, int tid, string pn, int line, string pos, bool g, int pid, int rp, int rs, bool sub, int? sfpid) :
      this(_gridDefault, gid, sid, tid, pn, line, pos, g, pid, rp, rs, sub, sfpid) // ctr6
    {
    }

    // ctr6
    public GameRoster(int grid, int gid, int sid, int tid, string pn, int line, string pos, bool g, int pid, int rp, int rs, bool sub, int? sfpid)
    {
      this.GameRosterId = grid;

      this.GameId = gid;
      this.TeamId = tid;
      this.SeasonId = sid;

      this.PlayerId = pid;
      this.PlayerNumber = pn;

      this.Line = line;
      this.Position = pos;
      this.RatingPrimary = rp;
      this.RatingSecondary = rs;
      this.Goalie = g;

      this.Sub = sub;
      this.SubbingForPlayerId = sfpid;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("grid: {0}, gid: {1}, tid: {2}, pn: {3}, sid: {4}",
                            this.GameRosterId,
                            this.GameId,
                            this.TeamId,
                            this.PlayerNumber,
                            this.SeasonId);

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