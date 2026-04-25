using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class RolePermissionsRepository(AppDbContext context) : IRolePermissionsRepository
{
    public List<Permission> GetPermissions(Role role)
    {
        return context.RolePermissions
            .FirstOrDefault(rp => rp.Role == role)?.Permissions ?? [];
    }
}
