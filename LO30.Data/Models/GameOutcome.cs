using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LO30.Data.Models
{
  public class GameOutcome
  {
    [Required, Key, Column(Order = 1), Index("PK2", 1, IsUnique = true)]
    public int GameId { get; set; }

    [Required, Key, Column(Order = 2)]
    public int TeamId { get; set; }

    [Required, Index("PK2", 2, IsUnique = true)]
    public bool HomeTeam { get; set; }

    [Required, MaxLength(1)]
    public string Outcome { get; set; }

    [Required]
    public int GoalsFor { get; set; }

    [Required]
    public int GoalsAgainst { get; set; }

    [Required]
    public int PenaltyMinutes { get; set; }

    [Required]
    public int Subs { get; set; }

    [Required]
    public bool Overriden { get; set; }

    [Required]
    public int OpponentTeamId { get; set; }

    [Required]
    public int SeasonId { get; set; }

    // virtual, foreign keys
    [ForeignKey("SeasonId")]
    public virtual Season Season { get; set; }

    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }

    [ForeignKey("OpponentTeamId")]
    public virtual Team OpponentTeam { get; set; }

    public GameOutcome()
    {
    }

    public GameOutcome(int sid, int tid, int gid, string res, int gf, int ga, int pim, bool over, int otid, int subs)
    {
      this.SeasonId = sid;
      this.TeamId = tid;
      this.GameId = gid;

      this.Outcome = res;
      this.GoalsFor = gf;
      this.GoalsAgainst = ga;
      this.PenaltyMinutes = pim;
      this.Overriden = over;
      this.OpponentTeamId = otid;
      this.Subs = subs;

      Validate();
    }

    private void Validate()
    {
      var locationKey = string.Format("sid: {0}, tid: {1}, gid: {2}",
                            this.SeasonId,
                            this.TeamId,
                            this.GameId);

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

      if (this.Overriden == false && this.GoalsFor > this.GoalsAgainst && this.Outcome != "W")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be a 'W' if GoalsFor > GoalsAgainst without an override for:" + locationKey, "Outcome");
      }

      if (this.Overriden == false && this.GoalsAgainst > this.GoalsFor && this.Outcome != "L")
      {
        throw new ArgumentException("Outcome (" + this.Outcome + ") must be a 'L' if GoalsAgainst > GoalsFor without an override for:" + locationKey, "Outcome");
      }

      if (this.Overriden == false && this.GoalsFor == this.GoalsAgainst && this.Outcome != "T")
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