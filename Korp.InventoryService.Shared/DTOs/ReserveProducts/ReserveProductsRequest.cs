using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.ReserveProducts;

public class ReserveProductsRequest
{
    public int InvoiceId { get; set; }

    [Required]
    public Guid Sagaid { get; set; }

    [Required]
    public string OriginId { get; set; } = null!;

    [Required]
    public string OriginType { get; set; } = null!;

    [MinLength(1, ErrorMessage = "At least one product item must be reserved.")]
    public List<ReserveProductRequest> Products = new();
}