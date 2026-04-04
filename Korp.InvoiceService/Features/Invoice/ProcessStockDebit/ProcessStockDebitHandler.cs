using Hangfire;
using Hangfire.Server;
using Korp.InvoiceService.Domain.Enums;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.Shared.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class ProcessStockDebitHandler(
    InventoryServiceHttpClient inventoryServiceHttpClient, 
    InvoiceDbContext invoiceDbContext,
    ILogger<ProcessStockDebitHandler> logger,
    RollbackProcessStockDebitHandler rollbackHandler)
{
    private readonly InventoryServiceHttpClient _inventoryServiceHttpClient = inventoryServiceHttpClient;
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly ILogger<ProcessStockDebitHandler> _logger = logger;
    private readonly RollbackProcessStockDebitHandler _rollbackHandler = rollbackHandler;

    private const int MaxRetryAttempts = 1;

    [AutomaticRetry(Attempts = MaxRetryAttempts, OnAttemptsExceeded = AttemptsExceededAction.Delete, DelaysInSeconds = new[] { 10, 20 })]
    public async Task HandleAsync([NotEmptyGuid] Guid sagaId, PerformContext? context)
    {
        var invoice = await _invoiceDbContext.Invoices
            .Where(i => i.SagaId == sagaId && i.InvoiceStatus == InvoiceStatusEnum.Processing)
            .FirstOrDefaultAsync();

        if (invoice is null)
        {
            _logger.LogError(
                "Attempt to process stock debit for non-existing invoice with SagaId {SagaId} or with status not equal to Processing", sagaId);
            return;
        }
        
        try
        {
            var result = await _inventoryServiceHttpClient.ReserveProductsAsync(invoice);

            if (result.IsSuccess)
            {
                invoice.MarkAsOpen();
                await _invoiceDbContext.SaveChangesAsync();

                return;
            }

            throw new Exception($"Reserva falhou para a Invoice {invoice.Id}: {result.Error}");
        }
        catch (Exception e)
        {
            int currentRetry = context?.GetJobParameter<int>("RetryCount") ?? 0;

            _logger.LogCritical(e, "Catastrophic error processing SagaId: {SagaId}", sagaId);

            if (currentRetry >= MaxRetryAttempts)
            {
                _logger.LogError("Esgotadas as tentativas do Hangfire para Saga {Id}. Cancelando.", sagaId);
                invoice.MarkAsCancelled();
                await _invoiceDbContext.SaveChangesAsync();

                using var _ = _rollbackHandler.HandleAsync(sagaId);
            }
            
            throw;
        }
    }
}
