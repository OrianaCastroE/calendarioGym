using System.Diagnostics.CodeAnalysis;
using DarkKitchen.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.API.Extensions;

[ExcludeFromCodeCoverage]
public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
