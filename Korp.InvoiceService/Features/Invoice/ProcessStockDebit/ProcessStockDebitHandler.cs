using Korp.InvoiceService.Features.Invoice.Domain.Enums;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class ProcessStockDebitHandler(
    InventoryServiceHttpClient _inventoryServiceHttpClient,
    InvoiceDbContext _invoiceDbContext) : IRequestHandlerAsync<ProcessStockDebitCommand, Result>
{
    public async Task<Result> HandleAsync(ProcessStockDebitCommand command, CancellationToken ct)
    {
        var invoice = await _invoiceDbContext.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(i => i.SagaId == command.SagaId, ct);

        if (invoice is null)
            return $"Invoice not found for SagaId {command.SagaId}";

        if (invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
            return "Invoice is CANCELLED. Cannot debit stock.";

        if (invoice.InvoiceStatus == InvoiceStatusEnum.Open || invoice.InvoiceStatus == InvoiceStatusEnum.Closed)
            return Result.Success();

        if (invoice.InvoiceStatus != InvoiceStatusEnum.Processing)
            return $"Invoice is in invalid status ({invoice.InvoiceStatus}). Cannot debit stock.";

        await _inventoryServiceHttpClient.ReserveProductsAsync(invoice);

        invoice.MarkAsOpen();
        await _invoiceDbContext.SaveChangesAsync(ct);

        return Result.Success();
    }
}
