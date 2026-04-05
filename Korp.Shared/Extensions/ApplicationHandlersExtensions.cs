using Korp.Sbared;
using Korp.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Korp.Shared.Extensions;

public static class ApplicationHandlersExtensions
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .SelectMany(t => t.GetInterfaces(), (handler, inter) => new { handler, inter })
            .Where(x => x.inter.IsGenericType &&
                        x.inter.GetGenericTypeDefinition() == typeof(IRequestHandlerAsync<,>));

        foreach (var item in handlerTypes)
            services.AddScoped(item.inter, item.handler);

        services.AddScoped<IDispatcher, ApplicationDispatcher>();

        return services;
    }

    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, ApplicationDispatcher>();
        return services;
    }
}
