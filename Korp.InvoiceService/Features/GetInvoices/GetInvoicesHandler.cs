using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs;
using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Features.GetInvoices;

public class GetInvoicesHandler(InvoiceDbContext invoiceDbContext)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;

    public async Task<List<GetInvoiceResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        return await _invoiceDbContext.Invoices
            .Include(i => i.InvoiceItems)
            .AsNoTracking()
            .Select(invoice => new GetInvoiceResponse()
            {
                Id = invoice.Id,
                InvoiceStatus = invoice.InvoiceStatus.ToString(),
                CreatedAt = invoice.CreatedAt,
                items = invoice.InvoiceItems.Select(ii => new GetInvoiceItemResponse
                {
                    ProductId = ii.ProductId,
                    Quantity = ii.Quantity
                })
            })
            .ToListAsync(cancellationToken);
    }
}
