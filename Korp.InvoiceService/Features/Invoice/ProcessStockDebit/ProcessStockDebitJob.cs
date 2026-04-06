using Hangfire;
using Hangfire.Server;
using Korp.Shared.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Korp.InvoiceService.Features.Invoice.ProcessStockDebit;

public class ProcessStockDebitJob(
    IDispatcher _dispatcher,
    IBackgroundJobClient _backgroundJobClient,
    ILogger<ProcessStockDebitJob> _logger)
{
    private const int MaxAttempts = 1;

    [AutomaticRetry(Attempts = MaxAttempts)]
    public async Task RunAsync(ProcessStockDebitCommand command, PerformContext? context)
    {
        try
        {
            var result = await _dispatcher.SendAsync(command);

            if (result.IsFailure)
            {
                _logger.LogWarning("Business failure for Saga {SagaId}: {Error}. Starting Rollback.", command.SagaId, result.Error);
                _backgroundJobClient.Enqueue<ProcessStockDebitJob>(x => x.RunRollbackAsync(command));
                return;
            }
        }
        catch (Exception e)
        {
            int retryCount = context?.GetJobParameter<int>("RetryCount") ?? 0;

            if (retryCount >= (MaxAttempts - 1))
            {
                _logger.LogCritical(e, "Final attempt failed for Saga {SagaId}. Starting Rollback.", command.SagaId);
                _backgroundJobClient.Enqueue<ProcessStockDebitJob>(x => x.RunRollbackAsync(command));
                return;
            }

            throw;
        }
    }

    [AutomaticRetry]
    public async Task RunRollbackAsync(ProcessStockDebitCommand command)
    {
        var result = await _dispatcher.SendAsync(new RollbackStockDebitCommand(command.SagaId));

        if (result.IsFailure)
            throw new Exception($"Rollback failed for Saga {command.SagaId}: {result.Error}");
    }
}
