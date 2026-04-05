using Korp.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Korp.Sbared;

public class ApplicationDispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandlerAsync<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(nameof(IRequestHandlerAsync<IRequest<TResponse>, TResponse>.HandleAsync));

        if (method == null)
            throw new InvalidOperationException("HandleAsync method not found in the Handler.");

        return await (Task<TResponse>) method.Invoke(handler, [request, cancellationToken])!;
    }
}
