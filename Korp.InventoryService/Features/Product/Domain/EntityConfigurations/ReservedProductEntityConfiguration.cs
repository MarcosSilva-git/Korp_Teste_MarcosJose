using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.InventoryService.Features.Product.Domain.EntityConfigurations;

public class ReservedProductEntityConfiguration : IEntityTypeConfiguration<ReservedProductEntity>
{
    public void Configure(EntityTypeBuilder<ReservedProductEntity> builder)
    {
        builder.ToTable("ReservedProducts");

        builder.HasKey(builder => builder.Id);

        builder.Property(builder => builder.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Version)
            .IsConcurrencyToken();

        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
