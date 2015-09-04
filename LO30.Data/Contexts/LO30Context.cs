using LO30.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LO30.Data.Contexts
{
  public class LO30Context : DbContext
  {
    public LO30Context()
      : base("LO30ReportingDB")
    {
      this.Configuration.LazyLoadingEnabled = false;
      this.Configuration.ProxyCreationEnabled = false;


      //Database.SetInitializer(new LO30ContextSeedInitializer());
      Database.SetInitializer(new MigrateDatabaseToLatestVersion<LO30Context, LO30MigrationsConfiguration>());
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

      //modelBuilder.Entity<Post>().HasRequired(p => p.Blog);

     /* modelBuilder.Entity<TeamStanding>()
                  .HasRequired(x => x.Team)
                  .WithRequiredDependent();*/
    }


    public DbSet<Division> Divisions { get; set; }

    //public DbSet<ForWebGoalieStat> ForWebGoalieStats { get; set; }
    //public DbSet<ForWebGoodThru> ForWebGoodThrus { get; set; }
    //public DbSet<ForWebPlayerStat> ForWebPlayerStats { get; set; }
    //public DbSet<ForWebTeamStanding> ForWebTeamStandings { get; set; }

    public DbSet<Game> Games { get; set; }
    //public DbSet<GameOutcome> GameOutcomes { get; set; }
    public DbSet<GameRoster> GameRosters { get; set; }
    //public DbSet<GameScore> GameScores { get; set; }
    public DbSet<GameTeam> GameTeams { get; set; }

    //public DbSet<GoalieStatCareer> GoalieStatCareers { get; set; }
    //public DbSet<GoalieStatGame> GoalieStatGames { get; set; }
    //public DbSet<GoalieStatSeason> GoalieStatSeasons { get; set; }
    //public DbSet<GoalieStatTeam> GoalieStatTeams { get; set; }

    public DbSet<Penalty> Penalties { get; set; }

    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerDraft> PlayerDrafts { get; set; }
    public DbSet<PlayerRating> PlayerRatings { get; set; }

    //public DbSet<PlayerStatCareer> PlayerStatCareers { get; set; }
    //public DbSet<PlayerStatGame> PlayerStatGames { get; set; }
    //public DbSet<PlayerStatSeason> PlayerStatSeasons { get; set; }
    //public DbSet<PlayerStatTeam> PlayerStatTeams { get; set; }

    public DbSet<PlayerStatus> PlayerStatuses { get; set; }
    public DbSet<PlayerStatusType> PlayerStatusTypes { get; set; }

    public DbSet<ScoreSheetEntry> ScoreSheetEntries { get; set; }
    public DbSet<ScoreSheetEntryProcessed> ScoreSheetEntriesProcessed { get; set; }
    public DbSet<ScoreSheetEntryPenalty> ScoreSheetEntryPenalties { get; set; }
    public DbSet<ScoreSheetEntryPenaltyProcessed> ScoreSheetEntryPenaltiesProcessed { get; set; }
    public DbSet<ScoreSheetEntrySub> ScoreSheetEntrySubs { get; set; }
    public DbSet<ScoreSheetEntrySubProcessed> ScoreSheetEntrySubsProcessed { get; set; }

    public DbSet<Season> Seasons { get; set; }

    public DbSet<Setting> Settings { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamRoster> TeamRosters { get; set; }
    public DbSet<TeamStanding> TeamStandings { get; set; }
  }
}