using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SignalR;

public static class SignalRServiceExtensions
{
    public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    public static IEndpointRouteBuilder MapCustomSignalR(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<NotificationHub>("/notificationHub");
        return endpoints;
    }
}
