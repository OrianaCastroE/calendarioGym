using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class RolePermissionsRepository(AppDbContext context) : IRolePermissionsRepository
{
    public List<Permission> GetPermissions(Role role)
    {
        return context.RolePermissions
            .Where(rp => rp.Role == role)
            .SelectMany(rp => rp.Permissions)
            .ToList();
    }
}
