using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class RolePermissionsServiceTest
{
    private readonly Role role = Role.Admin;
    private List<Permission>? permissions;
    private Mock<IRolePermissionsRepository>? rolePermissionsRepositoryMock;
    private RolePermissionsService? rolePermissionsService;

    [TestInitialize]
    public void Setup()
    {
        permissions =
        [
            Permission.CreateUser,
            Permission.UpdateUser,
            Permission.DeleteUser,
            Permission.GetUsers
        ];
        rolePermissionsRepositoryMock = new Mock<IRolePermissionsRepository>(MockBehavior.Strict);
        rolePermissionsService = new RolePermissionsService(rolePermissionsRepositoryMock.Object);
    }

    [TestMethod]
    public void GetPermissions_WhenRoleHasPermissions_ReturnsPermissions()
    {
        rolePermissionsRepositoryMock!.Setup(r => r.GetPermissions(role)).Returns(permissions!);

        var result = rolePermissionsService!.GetPermissions(role);

        CollectionAssert.AreEquivalent(permissions!, result);
    }
}
