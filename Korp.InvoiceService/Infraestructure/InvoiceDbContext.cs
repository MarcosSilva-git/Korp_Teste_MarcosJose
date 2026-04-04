using Korp.InvoiceService.Features.Invoice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Infraestructure;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceDbContext).Assembly);
    }

    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<InvoiceItemEntity> InvoiceItems { get; set; }
}
