using Microsoft.EntityFrameworkCore;
using ApiGateway.Models;

namespace ApiGateway.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // TABLAS
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CompraCab> CompraCab { get; set; }
        public DbSet<CompraDet> CompraDet { get; set; }
        public DbSet<VentaCab> VentaCab { get; set; }
        public DbSet<VentaDet> VentaDet { get; set; }
        public DbSet<MovimientoCab> MovimientoCab { get; set; }
        public DbSet<MovimientoDet> MovimientoDet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OPCIONAL: asegurar nombres exactos de tablas
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<CompraCab>().ToTable("CompraCab");
            modelBuilder.Entity<CompraDet>().ToTable("CompraDet");
            modelBuilder.Entity<VentaCab>().ToTable("VentaCab");
            modelBuilder.Entity<VentaDet>().ToTable("VentaDet");
            modelBuilder.Entity<MovimientoCab>().ToTable("MovimientoCab");
            modelBuilder.Entity<MovimientoDet>().ToTable("MovimientoDet");
        }
    }
}
