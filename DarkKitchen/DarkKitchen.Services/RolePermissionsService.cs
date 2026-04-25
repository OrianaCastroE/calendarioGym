using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;

namespace DarkKitchen.Services;

public class RolePermissionsService(IRolePermissionsRepository repository) : IRolePermissionsService
{
    private readonly IRolePermissionsRepository _repository = repository;

    public List<Permission> GetPermissions(Role role)
    {
        throw new NotImplementedException();
    }
}
