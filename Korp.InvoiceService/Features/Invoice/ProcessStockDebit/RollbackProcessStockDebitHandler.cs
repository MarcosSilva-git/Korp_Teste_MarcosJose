using Korp.InvoiceService.Features.Invoice.Domain.Enums;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class RollbackProcessStockDebitHandler(
    InvoiceDbContext _invoiceDbContext,
    InventoryServiceHttpClient _inventoryService,
    ILogger<RollbackProcessStockDebitHandler> _logger) : IRequestHandlerAsync<RollbackStockDebitCommand, Result>
{
    public async Task<Result> HandleAsync(RollbackStockDebitCommand command, CancellationToken ct)
    {
        var invoice = await _invoiceDbContext.Invoices
            .FirstOrDefaultAsync(i => i.SagaId == command.SagaId, ct);

        if (invoice is null)
        {
            var errorMessage = $"Invoice with SagaId {command.SagaId} not found.";
            _logger.LogError(errorMessage);

            return errorMessage;
        }

        if (invoice.InvoiceStatus == InvoiceStatusEnum.Closed || invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
            return Result.Success();

        if (invoice.InvoiceStatus != InvoiceStatusEnum.Processing)
        {
            var errorMessage = $"Invoice with SagaId {command.SagaId} is in invalid status ({invoice.InvoiceStatus}). Cannot rollback stock debit.";
            _logger.LogError(errorMessage);
            return errorMessage;
        }

        await _inventoryService.RollbackReserveProductsAsync(command.SagaId);

        invoice.MarkAsCancelled();
        await _invoiceDbContext.SaveChangesAsync(ct);

        return Result.Success();
    }
}
