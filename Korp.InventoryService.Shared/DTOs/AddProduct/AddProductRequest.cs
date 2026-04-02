using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.AddProduct;

public class AddProductRequest
{
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Balance should be greater then zero")]
    public int Balance { get; set; }
}
