using System.Diagnostics.CodeAnalysis;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.API.Extensions;

[ExcludeFromCodeCoverage]

// comment
public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
