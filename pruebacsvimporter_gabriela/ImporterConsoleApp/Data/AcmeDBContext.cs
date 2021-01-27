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
        public virtual DbSet<StockEntity> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
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

            modelBuilder.Entity<StockEntity>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DateStock).HasColumnType("date");              
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
