using Korp.InventoryService.Features.Product.Domain;
using Microsoft.EntityFrameworkCore;

namespace Korp.InventoryService.Infraestructure;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }

    public DbSet<ProductEntity> Products { get; set; }
}
