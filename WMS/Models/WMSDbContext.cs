using Microsoft.EntityFrameworkCore;
using WMS.Models.Entities;

namespace WMS.Models
{
    public class WMSDbContext : DbContext
    {
        //TODO: set connection string in app.config
        private string _conectionString = "Server=(localdb)\\localdb;Database=WMSDb;Trusted_Connection=True;";


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conectionString);
        }
    }
}
