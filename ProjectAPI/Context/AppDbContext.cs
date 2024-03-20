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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
<<<<<<< HEAD
<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
=======
            modelBuilder.Entity<User>().HasOne(u => u.Vehiculo).WithOne().HasForeignKey<User>(u => u.VehiculoId).IsRequired(false);

>>>>>>> 538434fa1740325ab4e900b1f2e7910213755360
            modelBuilder.Entity<User>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }



    }
}
