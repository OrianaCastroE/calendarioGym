using DarkKitchen.Domain.Enums;

namespace DarkKitchen.Domain.Interfaces.Service;

public interface IRolePermissionsService
{
    public List<Permission> GetPermissions(Role role);
}
