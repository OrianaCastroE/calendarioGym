using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class RolePermissionsRepository(AppDbContext context) : IRolePermissionsRepository
{
    public List<Permission> GetPermissions(Role role)
    {
        throw new NotImplementedException();
    }
}
