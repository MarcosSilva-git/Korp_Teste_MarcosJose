using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs;

public class UpdateProductRequest
{
    [MinLength(1)]
    public string Name { get; set; } = null!;
    
    [Range(1, int.MaxValue, ErrorMessage = "{0} should be greater then zero")]
    public int Stock { get; set; }
}
