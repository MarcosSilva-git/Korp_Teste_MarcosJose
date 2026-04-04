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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var propertyName = nameof(EntityBase.Version);
            var versionProp = entry.Metadata.FindProperty(propertyName);

            if (versionProp != null && versionProp.ClrType == typeof(int))
            {
                int currentVersion = (int)entry.Property(propertyName).CurrentValue!;
                entry.Property(propertyName).CurrentValue = currentVersion + 1;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ReservedProductEntity> ReservedProducts { get; set; }
}
