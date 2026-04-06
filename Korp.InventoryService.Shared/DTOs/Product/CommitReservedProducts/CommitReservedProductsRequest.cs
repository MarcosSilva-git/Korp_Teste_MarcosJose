using Korp.Shared.Abstractions;
using Korp.Shared.Attributes;
using Korp.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Korp.InventoryService.Shared.DTOs.Product.CommitReservedProducts;

public record CommitReservedProductsRequest([NotEmptyGuid] Guid SagaId) : IRequest<Result<CommitReservedProductsResponse, ValidationResult>>;
