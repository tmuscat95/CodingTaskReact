#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Cleverbit.CodingTask.Data.Models;
namespace Cleverbit.CodingTask.Data
{
    public partial class CodingTaskContext : DbContext
    {
        public CodingTaskContext()
        {
        }

        public CodingTaskContext(DbContextOptions<CodingTaskContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Guess> Guesses { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guess>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MatchNo })
                    .HasName("PK_Guesses_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Guesses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guesses_Matches");
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.Property(e => e.MatchNo).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}