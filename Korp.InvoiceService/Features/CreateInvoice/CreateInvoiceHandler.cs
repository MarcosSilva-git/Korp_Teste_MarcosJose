using Korp.InvoiceService.Domain.Entities;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.InvoiceService.Shared.DTOs.CreateInvoice;
using Korp.Shared.Abstractions;

namespace Korp.InvoiceService.Features.CreateInvoice;

public class CreateInvoiceHandler(
    InvoiceDbContext invoiceDbContext,
    InventoryServiceHttpClient inventoryServiceHttpClient)
{
    private readonly InvoiceDbContext _invoiceDbContext = invoiceDbContext;
    private readonly InventoryServiceHttpClient _inventoryServiceHttpClient = inventoryServiceHttpClient;

    public async Task<Result<int, string>> HandleAsync(CreateInvoiceRequest request)
    {
        if (request.InvoiceItems.Count == 0)
            return "Invoice must contain at least one item.";

        var items = request.InvoiceItems
            .Select((i, count) => new InvoiceItemEntity(i.ProductId, i.ProductName, i.Quantity, count + 1))
            .ToList();

        var newInvoice = new InvoiceEntity(items);

        _invoiceDbContext.Add(newInvoice);
        await _invoiceDbContext.SaveChangesAsync();

        //try
        //{
        //    await _inventoryServiceHttpClient.ReserveProductsAsync(newInvoice);
        //}
        //catch (Exception)
        //{
        //    throw;
        //}

        //_invoiceDbContext.Add(newInvoice);
        //await _invoiceDbContext.SaveChangesAsync();

        return newInvoice.Id;
    }
}
