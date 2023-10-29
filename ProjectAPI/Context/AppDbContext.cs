using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;

namespace ProjectAPI.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
        }

        
        
    }
}
