using Korp.InventoryService.Features.Product.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.InventoryService.Features.Product.Domain.EntityConfigurations;

public class ReservedProductEntityConfiguration : IEntityTypeConfiguration<ReservedProductEntity>
{
    public void Configure(EntityTypeBuilder<ReservedProductEntity> builder)
    {
        builder.ToTable("ReservedProducts");

        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.Id)
            .ValueGeneratedOnAdd();

        builder.Property(builder => builder.StockMovementStatus)
           .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<StockMovementStatusEnum>(e));

        builder.Property(p => p.Version)
            .IsConcurrencyToken();
    }
}
