using DarkKitchen.Domain.Enums;

namespace DarkKitchen.Domain.Entities;

public class RolePermissions
{
    public Role Role { get; set; }
    public List<Permission> Permissions { get; set; } = [];
}
