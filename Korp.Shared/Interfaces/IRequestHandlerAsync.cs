namespace Korp.Shared.Interfaces;

public interface IRequestHandlerAsync<TRequest, TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request);
}
