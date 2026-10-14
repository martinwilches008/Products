using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            // Definir id como PK
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .HasMaxLength(200) // Restricción de máximo 200 caracteres para Name
                .IsRequired(); // Campo requerido

            // Precisión del precio con un máximo de 18 digitos con 2 decimales
            entity.Property(p => p.Price)
                .HasPrecision(18, 2);
        });
    }
}