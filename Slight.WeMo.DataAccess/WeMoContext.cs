namespace Slight.WeMo.DataAccess
{
    using Microsoft.Data.Entity;

    using Slight.WeMo.Entities.Models;

    public class WeMoContext : DbContext
    {
        public DbSet<WeMoDeviceState> WeMoStates { get; set; }

        public DbSet<WeMoDevice> WeMoDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseFilePath = "WeMo.db";

            optionsBuilder.UseSqlite($"Data source={databaseFilePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
