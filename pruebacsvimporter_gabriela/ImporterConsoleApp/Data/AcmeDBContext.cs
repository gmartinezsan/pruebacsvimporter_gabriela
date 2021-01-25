using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ImporterConsoleApp.Models;

#nullable disable

namespace ImporterConsoleApp.Data
{
    public partial class AcmeDBContext : DbContext
    {
        public AcmeDBContext()
        {
        }

        public AcmeDBContext(DbContextOptions<AcmeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LogIngestion> LogIngestions { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<LogIngestion>(entity =>
            {
                entity.HasIndex(e => e.IngestionTimestamp, "IX_LogIngestions");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Error).IsUnicode(false);

                entity.Property(e => e.IngestionTimestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateStock).HasColumnType("date");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
