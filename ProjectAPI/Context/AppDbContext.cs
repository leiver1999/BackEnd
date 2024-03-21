using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;

namespace ProjectAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Requisicion> Requisiciones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Vehiculo)
                .WithOne().HasForeignKey<User>(u => u.VehiculoId)
                .IsRequired(false);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Requisicion)
                .WithMany(r => r.Facturas)
                .HasForeignKey(f => f.RequisicionId)
                .IsRequired(false);

            modelBuilder.Entity<Requisicion>()
                .HasOne(r => r.User)
                .WithMany(u => u.Requisiciones)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

            modelBuilder.Entity<User>().HasOne(u => u.Vehiculo).WithOne().HasForeignKey<User>(u => u.VehiculoId).IsRequired(false);

            modelBuilder.Entity<User>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }
    }
}
