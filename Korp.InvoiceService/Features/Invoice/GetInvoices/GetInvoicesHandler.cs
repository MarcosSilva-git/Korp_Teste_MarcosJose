using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Features.Invoice.GetInvoices;

public class GetInvoicesHandler(
    InvoiceDbContext invoiceDbContext, 
    ILogger<GetInvoicesHandler> logger) 
    : IRequestHandlerAsync<GetInvoicesQuery, Result<List<GetInvoiceResponse>, ValidationResult>>
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly ILogger<GetInvoicesHandler> _logger = logger;

    public async Task<Result<List<GetInvoiceResponse>, ValidationResult>> HandleAsync(GetInvoicesQuery query, CancellationToken cancellationToken)
    {
        if (query.Ids is null || string.IsNullOrWhiteSpace(query.Ids))
            return await GetInvoicesAsync(ids: null, cancellationToken);

        try
        {
            SortedSet<int> idSet = [.. query.Ids.Split(',').Select(int.Parse)];
            return await GetInvoicesAsync(idSet, cancellationToken);
        }
        catch (FormatException e)
        {
            _logger.LogWarning(e, "Failed to parse IDs from query string: {Ids}", query.Ids);
            return new ValidationResult("One or more IDs could not be converted to integers.", [nameof(query.Ids)]);
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
                   Status = invoice.InvoiceStatus.ToString(),
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