using Hangfire;
using Korp.InvoiceService.Features.Invoice.Domain.Entities;
using Korp.InvoiceService.Features.Invoice.ProcessStockDebit;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Korp.InvoiceService.Shared.DTOs.GetInvoices;
using Korp.Shared.Interfaces;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

public class CreateInvoiceHandler(
    IDispatcher _dispatcher,
    InvoiceDbContext _invoiceDbContext,
    IBackgroundJobClient _backgroundJobClient) : IRequestHandlerAsync<CreateInvoiceRequest, GetInvoiceResponse>
{
    public async Task<GetInvoiceResponse> HandleAsync(CreateInvoiceRequest request, CancellationToken ct)
    {
        var invoice = await AddPendingInvoice(request);
        var newProcessStockDebitCommand = new ProcessStockDebitCommand(invoice.SagaId);

        try
        {
            await _dispatcher.SendAsync(newProcessStockDebitCommand, ct);
        }
        catch (Exception)
        {
            _backgroundJobClient.Enqueue<ProcessStockDebitHandler>(
                x => x.HandleAsync(newProcessStockDebitCommand, ct));
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
