using Korp.InventoryService.Shared.DTOs.ReserveProducts;
using Korp.InvoiceService.Domain.Entities;
using Korp.Shared.Abstractions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Korp.InvoiceService.Infraestructure.Http;

public class InventoryServiceHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private const string ORIGIN_TYPE = "INVOICE_ID";

    public async Task<Result<bool, string>> ReserveProductsAsync(InvoiceEntity invoice)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/products/reserve");

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

        requestMessage.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
            return true;
            
        var errorContent = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return errorContent;

        return errorContent;
    }

    public async Task<Result<bool, string>> RollbackReserveProductsAsync(Guid sagaId)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/products/rollback")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(new { SagaId = sagaId.ToString() }),
                Encoding.UTF8,
                "application/json")
        };

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
            return true;

        var errorContent = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return errorContent;

        return errorContent;
    }
}