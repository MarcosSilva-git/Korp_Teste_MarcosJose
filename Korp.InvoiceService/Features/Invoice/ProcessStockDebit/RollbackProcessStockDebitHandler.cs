using Hangfire;
using Hangfire.Server;
using Korp.InvoiceService.Domain.Enums;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.Shared.Attributes;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class RollbackProcessStockDebitHandler(
    InvoiceDbContext invoiceDbContext,
    InventoryServiceHttpClient inventoryService,
    ILogger<RollbackProcessStockDebitHandler> logger)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly InventoryServiceHttpClient _inventoryService = inventoryService;
    private readonly ILogger<RollbackProcessStockDebitHandler> _logger = logger;

    [AutomaticRetry(Attempts = 10)]
    public async Task HandleAsync([NotEmptyGuid] Guid sagaId, PerformContext? context)
    {
        var invoice = await _invoiceDbContext.Invoices
            .Where(i => i.SagaId == sagaId 
                && i.InvoiceStatus == InvoiceStatusEnum.Processing)
            .FirstOrDefaultAsync();

        if (invoice is null)
        {
            _logger.LogError(
                "Attempt to rollback stock debit for non-existing invoice with SagaId {SagaId} or with status not equal to Processing", sagaId);
            return;
        }

        try
        {
            var result = await _inventoryService.RollbackReserveProductsAsync(sagaId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Rollback of sagaid {sagaid} was completed.", sagaId);

                invoice.MarkAsCancelled();
                await _invoiceDbContext.SaveChangesAsync();

                return;
            }

            throw new Exception($"Rollback failed to Invoice {invoice.Id}: {result.Error}");
        }
        catch (Exception e)
        {
            int? currentRetry = context?.GetJobParameter<int>("RetryCount");
            _logger.LogError(e, "Number of trys: {currentRetry}", currentRetry is null ? 1 : currentRetry + 2);
            currentRetry ??= 0;

            throw;
        }
    }
}
