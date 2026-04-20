using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;

namespace DarkKitchen.Tests.Domain.Entities;

[TestClass]
public class RolePermissionsTest
{
    private readonly Role role = Role.Admin;
    private RolePermissions? rolePermissions;

    [TestInitialize]
    public void Setup()
    {
        rolePermissions = new RolePermissions()
        {
            Role = role,
            Permissions =
            [
                Permission.CreateUser,
                Permission.UpdateUser,
                Permission.DeleteUser,
                Permission.GetUsers
            ]
        };
    }

    [TestMethod]
    public void RolePermissions_WithValidData_HasCorrectRole()
    {
        var roleCheck = rolePermissions.Role;

        Assert.AreEqual(role, roleCheck);
    }

    [TestMethod]
    public void RolePermissions_WithValidData_HasCorrectPermissions()
    {
        var permissionsCheck = rolePermissions.Permissions;

        Assert.AreEqual(4, permissionsCheck.Count);
    }
}
