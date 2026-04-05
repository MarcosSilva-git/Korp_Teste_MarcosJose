using Hangfire;
using Hangfire.Server;
using Korp.Shared.Interfaces;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class ProcessStockDebitJob(
    IDispatcher _dispatcher,
    IBackgroundJobClient _backgroundJobClient,
    ILogger<ProcessStockDebitJob> _logger)
{
    private const int MaxAttempts = 10;

    [AutomaticRetry(Attempts = MaxAttempts)]
    public async Task RunAsync(Guid sagaId, PerformContext? context)
    {
        try
        {
            var result = await _dispatcher.SendAsync(new ProcessStockDebitCommand(sagaId));

            if (result.IsFailure)
            {
                _logger.LogWarning("Business failure for Saga {SagaId}: {Error}. Starting Rollback.", sagaId, result.Error);
                _backgroundJobClient.Enqueue<ProcessStockDebitJob>(x => x.RunRollbackAsync(sagaId));
                return;
            }
        }
        catch (Exception e)
        {
            int retryCount = context?.GetJobParameter<int>("RetryCount") ?? 0;

            if (retryCount >= (MaxAttempts - 1))
            {
                _logger.LogCritical(e, "Final attempt failed for Saga {SagaId}. Starting Rollback.", sagaId);
                _backgroundJobClient.Enqueue<ProcessStockDebitJob>(x => x.RunRollbackAsync(sagaId));
            }

            throw;
        }
    }

    [AutomaticRetry]
    public async Task RunRollbackAsync(Guid sagaId)
    {
        var result = await _dispatcher.SendAsync(new RollbackStockDebitCommand(sagaId));

        if (result.IsFailure)
            throw new Exception($"Rollback failed for Saga {sagaId}: {result.Error}");
    }
}
