using DarkKitchen.Domain.Enums;

namespace DarkKitchen.Domain.Interfaces.Repository;

public interface IRolePermissionsRepository
{
    public List<Permission> GetPermissions(Role role);
}
