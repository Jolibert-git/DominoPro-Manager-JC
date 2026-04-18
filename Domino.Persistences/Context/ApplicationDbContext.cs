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
                //entity.HasKey(p => p.Id);

                //entity.Property(p => p.Name)
                //      .IsRequired()
                //      .HasMaxLength(50);

                //entity.Property(p => p.LastName)
                //      .IsRequired()
                //      .HasMaxLength(50);

                //entity.Property(p => p.Email)
                //      .HasMaxLength(100);

                //entity.Property(p => p.Phone)
                //      .HasMaxLength(20);

                entity.HasIndex(p => p.Email)
                  .IsUnique()
                  .HasFilter("[Email] IS NOT NULL AND [Email] != ''");

            //entity.Ignore(p => p.WinRate);
        });


            //Tournament
            modelBuilder.Entity<Tournament>(entity =>
            {
                //entity.HasKey(t => t.Id);

                //entity.Property(t => t.Name)
                //      .IsRequired()
                //      .HasMaxLength(50);

                //entity.Property(t => t.Description)
                //      .HasMaxLength(500);

                //entity.Property(t => t.Place)
                //      .HasMaxLength(100);

                //entity.Property(t => t.Prize)
                //      .HasMaxLength(200);

                entity.Property(t => t.Mode)
                      .HasConversion<string>();

                entity.Property(t => t.Status)
                      .HasConversion<string>();

                //entity.Ignore(t => t.TotalRegistered);
                //entity.Ignore(t => t.IsFull);
            });

            
            //TournamentRegistration
            modelBuilder.Entity<TournamentRegistration>(entity =>
            {
                //entity.HasKey(r => r.Id);

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
                //entity.HasKey(r => r.Id);

                entity.Property(r => r.Status)
                      .HasConversion<string>();

                //entity.Ignore(r => r.Duration);
                //entity.Ignore(r => r.TotalTable);
                //entity.Ignore(r => r.CompleteTable);

                entity.HasOne(r => r.Tournament)
                      .WithMany(t => t.Rondas)
                      .HasForeignKey(r => r.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //Table
            modelBuilder.Entity<Table>(entity =>
            {
                //entity.HasKey(t => t.Id);

                entity.Property(t => t.Status)
                      .HasConversion<string>();

                //entity.Ignore(t => t.Duration);
                //entity.Ignore(t => t.IsComplete);
                //entity.Ignore(t => t.Winner);

                entity.HasOne(t => t.Round)
                      .WithMany(r => r.Tables)
                      .HasForeignKey(t => t.RoundId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                //entity.HasKey(r => r.Id);

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
                //entity.HasKey(c => c.Id);

                entity.HasIndex(c => new { c.PlayerId, c.TournamentId })
                      .IsUnique();

                //entity.Ignore(c => c.TotalGame);
                //entity.Ignore(c => c.WinRate);
                //entity.Ignore(c => c.PointsDifferent);

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
