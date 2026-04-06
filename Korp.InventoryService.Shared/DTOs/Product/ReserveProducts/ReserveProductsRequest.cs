using Korp.Shared.Abstractions;
using Korp.Shared.Attributes;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;

public class ReserveProductsRequest : IRequest<Result<bool, List<ValidationResult>>>
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