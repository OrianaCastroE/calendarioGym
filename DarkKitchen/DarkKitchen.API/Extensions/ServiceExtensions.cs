using System.Diagnostics.CodeAnalysis;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Services;

namespace DarkKitchen.API.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IRolePermissionsService, RolePermissionsService>();
        services.AddScoped<IShippingTypeService, ShippingTypeService>();
        return services;
    }
}
