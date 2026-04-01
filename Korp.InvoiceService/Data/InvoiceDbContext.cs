using Korp.InvoiceService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Data;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceDbContext).Assembly);
    }

    DbSet<InvoiceEntity> Invoices { get; set; }
    DbSet<InvoiceItemEntity> InvoiceItems { get; set; }
}
