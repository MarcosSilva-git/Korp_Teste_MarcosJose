namespace Korp.Shared.Interfaces;

public interface IRequestHandlerAsync<TRequest, TResponse> : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancelationToken = default);
}