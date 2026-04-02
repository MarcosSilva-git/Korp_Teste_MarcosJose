using Korp.InvoiceService.Domain.Entities;
using Korp.InvoiceService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.InvoiceService.Infraestructure.EntityConfigurations;

public class InvoiceEntityConfiguration : IEntityTypeConfiguration<InvoiceEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedOnAdd();

        builder.Property(builder => builder.InvoiceStatus)
           .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<InvoiceStatusEnum>(e)
            );

        builder.HasQueryFilter(p => p.DeletedAt == null);
    }
}
