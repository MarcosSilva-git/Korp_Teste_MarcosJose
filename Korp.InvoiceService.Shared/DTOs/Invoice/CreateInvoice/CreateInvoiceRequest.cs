using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.Invoice.CreateInvoice;

public class CreateInvoiceRequest : IRequest<CreateInvoiceResponse>
{
    [MinLength(1, ErrorMessage = "{0} must contain at least one item.")]
    public List<CreateInvoiceitemRequest> Items { get; set; } = new();
}

public class CreateInvoiceitemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ProductId should be greater then zero")]
    public int ProductId { get; set; }

    [MinLength(1, ErrorMessage = "Product name must be at least 1 character long.")]
    public string ProductName { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Balance should be greater then zero")]
    public int Quantity { get; set; }
}