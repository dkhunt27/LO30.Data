using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Objects
{
  public class GameOutcome
  {
    [Required, Key, Column(Order = 1), ForeignKey("Game")]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2), ForeignKey("Team")]
    public int TeamId { get; set; }

    [Required, MaxLength(1)]
    public string Outcome { get; set; }

    [Required]
    public int GoalsFor { get; set; }

    [Required]
    public int GoalsAgainst { get; set; }

    [Required]
    public int PenaltyMinutes { get; set; }

    public int Subs { get; set; }

    [Required]
    public bool Override { get; set; }

    [Required, ForeignKey("OpponentTeam")]
    public int OpponentTeamId { get; set; }

    [Required, ForeignKey("Season")]
    public int SeasonId { get; set; }

    public virtual Game Game { get; set; }
    public virtual Team Team { get; set; }
    public virtual Team OpponentTeam { get; set; }
    public virtual Season Season { get; set; }

    public GameOutcome()
    {
    }

    public GameOutcome(int gid, int sid, int tid, string res, int gf, int ga, int pim, bool over, int otid)
      : this(gid, sid, tid, res, gf, ga, pim, over, otid, 0)
    { }

    public GameOutcome(int gid, int sid, int tid, string res, int gf, int ga, int pim, bool over, int otid, int subs)
    {
      this.GameId = gid;
      this.SeasonId = sid;
      this.TeamId = tid;

      this.Outcome = res;
      this.GoalsFor = gf;
      this.GoalsAgainst = ga;
      this.PenaltyMinutes = pim;
      this.Override = over;
      this.OpponentTeamId = otid;
      this.Subs = subs;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("gid: {0}, tid: {1}, sid: {2}",
                            this.GameId,
                            this.TeamId,
                            this.SeasonId);

      if (this.GoalsFor < 0)
      {
        throw new ArgumentException("GoalsFor (" + this.GoalsFor + ") must be a positive number for:" + locationKey, "GoalsFor");
      }

      if (this.GoalsAgainst < 0)
      {
        throw new ArgumentException("GoalsAgainst (" + this.GoalsAgainst + ") must be a positive number for:" + locationKey, "GoalsAgainst");
      }

      if (this.PenaltyMinutes < 0)
      {
        throw new ArgumentException("PenaltyMinutes (" + this.PenaltyMinutes + ") must be a positive number for:" + locationKey, "PenaltyMinutes");
      }

      if (this.Outcome != "W" && this.Outcome != "L" && this.Outcome != "T")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be 'W','L', or 'T' for:" + locationKey, "Outcome");
      }

      if (this.Override == false && this.GoalsFor > this.GoalsAgainst && this.Outcome != "W")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be a 'W' if GoalsFor > GoalsAgainst without an override for:" + locationKey, "Outcome");
      }

      if (this.Override == false && this.GoalsAgainst > this.GoalsFor && this.Outcome != "L")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be a 'L' if GoalsAgainst > GoalsFor without an override for:" + locationKey, "Outcome");
      }

      if (this.Override == false && this.GoalsFor == this.GoalsAgainst && this.Outcome != "T")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be a 'T' if GoalsFor = GoalsAgainst without an override for:" + locationKey, "Outcome");
      }

      if (this.TeamId == this.OpponentTeamId)
      {
        throw new ArgumentException("OpponentTeamId (" + this.OpponentTeamId + ") cannot equal TeamId for:" + locationKey, "OpponentTeamId");
      }

      if (this.Subs < 0)
      {
        throw new ArgumentException("Subs (" + this.Subs + ") must be a positive number for:" + locationKey, "Subs");
      }
    }
  }
}