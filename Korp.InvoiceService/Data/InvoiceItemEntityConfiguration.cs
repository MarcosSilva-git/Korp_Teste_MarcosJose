using Korp.InvoiceService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.InvoiceService.Data;

public class InvoiceItemEntityConfiguration : IEntityTypeConfiguration<InvoiceItemEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemEntity> builder)
    {
        builder.ToTable("InvoiceItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
