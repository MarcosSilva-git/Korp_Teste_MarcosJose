using Korp.Shared.Abstractions;
using Korp.Shared.Attributes;
using Korp.Shared.Interfaces;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public record RollbackStockDebitCommand([NotEmptyGuid] Guid SagaId) : IRequest<Result>;
