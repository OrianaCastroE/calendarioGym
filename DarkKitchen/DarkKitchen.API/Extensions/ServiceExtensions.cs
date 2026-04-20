using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Services;

namespace DarkKitchen.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPromotionService, PromotionService>();
        return services;
    }
}
