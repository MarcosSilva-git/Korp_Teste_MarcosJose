using System.ComponentModel.DataAnnotations;

namespace Korp.InvoiceService.Shared.DTOs.CreateInvoice;

public class CreateInvoiceitemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ProductId should be greater then zero")]
    public int ProductId { get; set; }

    [MinLength(1, ErrorMessage = "Product name must be at least 1 character long.")]
    public string ProductName { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Balance should be greater then zero")]
    public int Quantity { get; set; }
}
