using Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;
using Korp.InvoiceService.Features.Invoice.Domain.Entities;

namespace Korp.InvoiceService.Infraestructure.Http;

public class InventoryServiceHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private const string ORIGIN_TYPE = "INVOICE_ID";

    public async Task ReserveProductsAsync(InvoiceEntity invoice)
    {
        var body = new ReserveProductsRequest
        {
            Sagaid = invoice.SagaId,
            OriginId = invoice.Id.ToString(),
            OriginType = ORIGIN_TYPE,
            Products = invoice.InvoiceItems.Select(item => new ReserveProductRequest
            {
                Id = item.ProductId,
                Quantity = item.Quantity
            }).ToList(),
        };

        var response = await _httpClient.PostAsJsonAsync("/api/products/reserve", body);

        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadAsStringAsync();

        throw new Exception($"Inventory Service failure ({response.StatusCode}): {error ?? "No message"}");
    }

    public async Task RollbackReserveProductsAsync(Guid sagaId)
    {
        var body = new RollbackReserveProductsRequest { SagaId = sagaId };

        var response = await _httpClient.PostAsJsonAsync("/api/products/reserve/rollback", body);

        if (response.IsSuccessStatusCode)
            return;

        var error = await response.Content.ReadAsStringAsync();

        throw new Exception($"Critical Inventory Service Rollback Error [{response.StatusCode}] for Saga {sagaId}: {error}");
    }
}