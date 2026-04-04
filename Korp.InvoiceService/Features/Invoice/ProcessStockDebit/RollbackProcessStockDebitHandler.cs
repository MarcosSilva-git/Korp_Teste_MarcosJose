using Korp.Shared.Attributes;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class RollbackProcessStockDebitHandler
{
    public async Task HandleAsync([NotEmptyGuid] Guid sagaId)
    {

    }
}
