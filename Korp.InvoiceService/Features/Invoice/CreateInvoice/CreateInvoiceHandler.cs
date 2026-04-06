using Hangfire;
using Korp.InvoiceService.Features.Invoice.Domain.Entities;
using Korp.InvoiceService.Features.Invoice.ProcessStockDebit;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Shared.DTOs.Invoice.CreateInvoice;
using Korp.Shared.Interfaces;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

public class CreateInvoiceHandler(
    IDispatcher _dispatcher,
    InvoiceDbContext _invoiceDbContext,
    IBackgroundJobClient _backgroundJobClient) : IRequestHandlerAsync<CreateInvoiceRequest, CreateInvoiceResponse>
{
    public async Task<CreateInvoiceResponse> HandleAsync(CreateInvoiceRequest request, CancellationToken ct)
    {
        var invoice = await AddPendingInvoice(request);

        try
        {
            await _dispatcher.SendAsync(new ProcessStockDebitCommand(invoice.SagaId), ct);
        }
        catch (Exception)
        {
            _backgroundJobClient.Enqueue<ProcessStockDebitJob>(
                x => x.RunAsync(new ProcessStockDebitCommand(invoice.SagaId), null));
        }


        return new CreateInvoiceResponse(invoice.Id);
    }

    public async Task<InvoiceEntity> AddPendingInvoice(CreateInvoiceRequest request)
    {
        var items = request.Items
            .Select((i, count) => new InvoiceItemEntity(i.ProductId, i.ProductName, i.Quantity, count + 1))
            .ToList();

        var newInvoice = new InvoiceEntity(items);

        _invoiceDbContext.Add(newInvoice);
        await _invoiceDbContext.SaveChangesAsync();

        return newInvoice;
    }
}
