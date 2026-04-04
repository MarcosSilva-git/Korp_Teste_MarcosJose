using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.ReserveProducts;

public class ReserveProductRequest
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }
}
