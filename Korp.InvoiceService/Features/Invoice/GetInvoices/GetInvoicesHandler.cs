using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Korp.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Features.Invoice.GetInvoices;

public class GetInvoicesHandler(InvoiceDbContext invoiceDbContext, ILogger<GetInvoicesHandler> logger)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly ILogger<GetInvoicesHandler> _logger = logger;

    public async Task<Result<List<GetInvoiceResponse>, ValidationResult>> HandleAsync(string? ids, CancellationToken cancellationToken = default)
    {
        if (ids is null || string.IsNullOrWhiteSpace(ids))
            return await GetInvoicesAsync(ids: null, cancellationToken);

        try
        {
            SortedSet<int> idSet = [.. ids.Split(',').Select(int.Parse)];
            return await GetInvoicesAsync(idSet, cancellationToken);
        }
        catch (FormatException e)
        {
            _logger.LogWarning(e, "Failed to parse IDs from query string: {Ids}", ids);
            return new ValidationResult("One or more IDs could not be converted to integers.", [nameof(ids)]);
        }

    }

    private async Task<List<GetInvoiceResponse>> GetInvoicesAsync(SortedSet<int>? ids, CancellationToken cancellationToken)
    {
        var query = _invoiceDbContext.Invoices
               .AsNoTracking();

        if (ids != null)
            query = query.Where(p => ids.Contains(p.Id));

        return await query
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