using Korp.Shared.Abstractions;
using Korp.Shared.Attributes;
using Korp.Shared.Interfaces;

namespace Korp.InventoryService.Shared.DTOs.Product.ReserveProducts;

public class RollbackReserveProductsRequest : IRequest<Result>
{
    [NotEmptyGuid]
    public Guid SagaId { get; set; }
}
