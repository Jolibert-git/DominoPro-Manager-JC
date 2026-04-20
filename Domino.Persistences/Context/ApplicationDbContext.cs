using Domino.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domino.Persistence.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domino.Domain.Entities.Tournament> Tournaments { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<TournamentRegistration> Registrations { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<Classification> Classifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //  Player
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasIndex(p => p.Email)
                  .IsUnique()
                  .HasFilter("[Email] IS NOT NULL AND [Email] != ''");
            

       
            });


            //Tournament
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(t => t.Mode)
                      .HasConversion<string>();

                entity.Property(t => t.Status)
                      .HasConversion<string>();

               
            });

            
            //TournamentRegistration
            modelBuilder.Entity<TournamentRegistration>(entity =>
            {

                entity.Property(r => r.Status)
                      .HasConversion<string>();

                entity.HasIndex(r => new { r.PlayerId, r.TournamentId })
                      .IsUnique();

                entity.HasOne(r => r.Player)
                      .WithMany(p => p.Registration)
                      .HasForeignKey(r => r.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Tournament)
                      .WithMany(t => t.Registrations)
                      .HasForeignKey(r => r.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //  Round
            modelBuilder.Entity<Round>(entity =>
            {

                entity.Property(r => r.Status)
                      .HasConversion<string>();

                entity.HasOne(r => r.Tournament)
                      .WithMany(t => t.Rondas)
                      .HasForeignKey(r => r.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Table
            modelBuilder.Entity<Table>(entity =>
            {

                entity.Property(t => t.Status)
                      .HasConversion<string>();

                entity.HasOne(t => t.Round)
                      .WithMany(r => r.Tables)
                      .HasForeignKey(t => t.RoundId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Result>(entity =>
            {

                entity.HasOne(r => r.Player)
                      .WithMany(p => p.Results)       
                      .HasForeignKey(r => r.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Playmate)
                      .WithMany()                    
                      .HasForeignKey(r => r.PlaymateId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Table)
                      .WithMany(t => t.Results)
                      .HasForeignKey(r => r.TableId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Classification
            modelBuilder.Entity<Classification>(entity =>
            {
                entity.HasIndex(c => new { c.PlayerId, c.TournamentId })
                      .IsUnique();

                entity.HasOne(c => c.Player)
                      .WithMany(p => p.Classifications)
                      .HasForeignKey(c => c.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Tournament)
                      .WithMany(t => t.Classificationes)
                      .HasForeignKey(c => c.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
