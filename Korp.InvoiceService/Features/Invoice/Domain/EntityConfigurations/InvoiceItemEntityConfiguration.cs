using Korp.InvoiceService.Features.Invoice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.InvoiceService.Features.Invoice.Domain.EntityConfigurations;

public class InvoiceItemEntityConfiguration : IEntityTypeConfiguration<InvoiceItemEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemEntity> builder)
    {
        builder.ToTable("InvoiceItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
