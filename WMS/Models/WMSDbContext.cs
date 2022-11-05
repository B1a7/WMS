using Microsoft.EntityFrameworkCore;
using WMS.Models.Entities;

namespace WMS.Models
{
    public class WMSDbContext : DbContext
    {
        //TODO: set connection string in app.config
        private string _conectionString = "Server=(localdb)\\localdb;Database=WMSDb;Trusted_Connection=True;";

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }   

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conectionString);
        }
    }
}
