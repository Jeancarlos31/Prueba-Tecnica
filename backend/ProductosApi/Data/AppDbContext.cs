using Microsoft.EntityFrameworkCore;
using ProductosApi.Models;

namespace ProductosApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
        });

        // Datos semilla para poder probar el API/frontend inmediatamente.
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop Dell XPS 13",
                Description = "Laptop ultraliviana de 13 pulgadas, 16GB RAM, 512GB SSD",
                Price = 1299.99m,
                Stock = 15,
                CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Mouse Logitech MX Master 3",
                Description = "Mouse inalámbrico ergonómico de alta precisión",
                Price = 99.99m,
                Stock = 40,
                CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "Teclado Mecánico Keychron K8",
                Description = "Teclado mecánico inalámbrico con switches marrones",
                Price = 89.50m,
                Stock = 25,
                CreatedAt = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
