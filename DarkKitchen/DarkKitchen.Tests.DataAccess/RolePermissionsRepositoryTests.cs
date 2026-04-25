using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class RolePermissionsRepositoryTests
{
    private RolePermissions? rolePermissions;
    private AppDbContext? _context;
    private RolePermissionsRepository? _rolePermissionsRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        rolePermissions = new RolePermissions
        {
            Role = Role.Admin,
            Permissions =
            [
                Permission.CreateUser,
                Permission.UpdateUser,
                Permission.DeleteUser,
                Permission.GetUsers
            ]
        };
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new AppDbContext(options);
        _rolePermissionsRepository = new RolePermissionsRepository(_context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void GetPermissions_WhenRoleExists_ReturnsPermissions()
    {
        _context.RolePermissions.Add(rolePermissions!);
        _context.SaveChanges();

        var result = _rolePermissionsRepository.GetPermissions(Role.Admin);

        Assert.AreEqual(4, result.Count);
    }

    [TestMethod]
    public void GetPermissions_WhenRoleDoesNotExist_ReturnsEmptyList()
    {
        var result = _rolePermissionsRepository.GetPermissions(Role.Admin);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}
