using Hangfire;
using Korp.InvoiceService.Domain.Entities;
using Korp.InvoiceService.Features.Invoice.ProcessStockDebit;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.InvoiceService.Migrations;
using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Korp.Shared.Abstractions;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

public class CreateInvoiceHandler(
    InvoiceDbContext invoiceDbContext,
    InventoryServiceHttpClient inventoryServiceHttpClient,
    ProcessStockDebitHandler processStockDebitHandler,
    IBackgroundJobClient backgroundJobClient)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly InventoryServiceHttpClient _inventoryServiceHttpClient = inventoryServiceHttpClient;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    private readonly ProcessStockDebitHandler _processStockDebitHandler = processStockDebitHandler;

    public async Task<GetInvoiceResponse> HandleAsync(CreateInvoiceRequest request)
    {
        var invoice = await AddPendingInvoice(request);

        try
        {
            await _processStockDebitHandler.HandleAsync(invoice.SagaId, null);
        }
        catch (Exception)
        {
            _backgroundJobClient.Enqueue<ProcessStockDebitHandler>(x => x.HandleAsync(invoice.SagaId, null));
        }

        return new GetInvoiceResponse()
        {
            Id = invoice.Id,
            CreatedAt = invoice.CreatedAt,
            Status = invoice.InvoiceStatus.ToString(),
            items = invoice.InvoiceItems.Select(i => new GetInvoiceItemResponse()
            {
                Id = i.Id,
                Quantity = i.Quantity,
                ProductId = i.ProductId,
                CreatedDate = i.CreatedAt,
                ProductName = i.ProductName,
                ItemSequence = i.ItemSequence,
            }),
        };
    }

    public async Task<InvoiceEntity> AddPendingInvoice(CreateInvoiceRequest request)
    {
        var items = request.InvoiceItems
            .Select((i, count) => new InvoiceItemEntity(i.ProductId, i.ProductName, i.Quantity, count + 1))
            .ToList();

        var newInvoice = new InvoiceEntity(items);

        _invoiceDbContext.Add(newInvoice);
        await _invoiceDbContext.SaveChangesAsync();

        return newInvoice;
    }
}
