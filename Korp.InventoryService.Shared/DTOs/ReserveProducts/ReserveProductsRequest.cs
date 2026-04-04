using Korp.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.ReserveProducts;

public class ReserveProductsRequest
{
    [NotEmptyGuid]
    public Guid Sagaid { get; set; }

    [Required]
    public string OriginId { get; set; } = null!;

    [Required]
    public string OriginType { get; set; } = null!;

    [MinLength(1, ErrorMessage = "At least one product item must be reserved.")]
    public List<ReserveProductRequest> Products { get; set; } = new();
}