using Korp.Shared.Abstractions;
using Korp.Shared.Interfaces;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public record ProcessStockDebitCommand(Guid SagaId) : IRequest<Result>;
