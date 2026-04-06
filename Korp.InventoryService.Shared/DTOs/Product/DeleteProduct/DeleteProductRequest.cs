using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.Product.DeleteProduct;

public record DeleteProductRequest(int ProductId) : IRequest<Result<DeleteProductResponse, ValidationResult>>;
