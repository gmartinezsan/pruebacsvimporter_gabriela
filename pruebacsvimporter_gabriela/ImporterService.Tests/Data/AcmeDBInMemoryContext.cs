using ImporterConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CobblerBackendExercise.Data
{
    public class AcmeDbContext : DbContext
    {
    
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("AcmeDb");
        public DbSet<StockEntity> Stocks { get; set; }

        public DbSet<LogIngestion> LogIngestions { get; set; }
    }
}