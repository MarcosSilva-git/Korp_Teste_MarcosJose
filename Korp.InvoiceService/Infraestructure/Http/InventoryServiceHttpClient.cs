using Korp.InvoiceService.Domain.Entities;
using Korp.Shared.Abstractions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Korp.InvoiceService.Infraestructure.Http;

public class InventoryServiceHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<bool, string>> ReserveProductsAsync(InvoiceEntity invoice)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/products/reserve");

        var body = new
        {
            Invoiceid = invoice.Id,
            items = invoice.InvoiceItems.Select(item => new
            {
                item.ProductId,
                item.Quantity
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
}