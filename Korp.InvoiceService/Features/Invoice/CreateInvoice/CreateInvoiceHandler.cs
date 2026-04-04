using Korp.InvoiceService.Domain.Entities;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Korp.Shared.Abstractions;

namespace Korp.InvoiceService.Features.Invoice.CreateInvoice;

public class CreateInvoiceHandler(
    InvoiceDbContext invoiceDbContext,
    InventoryServiceHttpClient inventoryServiceHttpClient)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly InventoryServiceHttpClient _inventoryServiceHttpClient = inventoryServiceHttpClient;

    public async Task<Result<int, string>> HandleAsync(CreateInvoiceRequest request)
    {
        var invoice = await AddPendingInvoice(request);

        try
        {
            var result = await _inventoryServiceHttpClient.ReserveProductsAsync(invoice);

            if (result.IsSuccess)
            {
                invoice.MarkAsOpen();
                await _invoiceDbContext.SaveChangesAsync();
                
                return invoice.Id;
            }

            // If reservation fails, we can choose to either keep the invoice as pending or delete it.
        }
        catch (Exception)
        {
            throw;
        }


        //return invoice.Id;
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
