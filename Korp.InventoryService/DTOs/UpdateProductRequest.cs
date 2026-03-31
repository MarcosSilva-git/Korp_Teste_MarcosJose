using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.DTOs;

public class UpdateProductRequest
{
    [MinLength(1)]
    public string Name { get; set; } = null!;
    
    [Range(1, int.MaxValue, ErrorMessage = "Balance should be greater then zero")]
    public int Balance { get; set; }
}
