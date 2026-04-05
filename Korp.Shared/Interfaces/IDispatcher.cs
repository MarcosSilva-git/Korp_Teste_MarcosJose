namespace Korp.Shared.Interfaces;

public interface IDispatcher
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken ct = default);
}
