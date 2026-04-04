using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.AddProduct;

public class AddProductRequest
{
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} should be greater then zero")]
    public int Stock { get; set; }
}
