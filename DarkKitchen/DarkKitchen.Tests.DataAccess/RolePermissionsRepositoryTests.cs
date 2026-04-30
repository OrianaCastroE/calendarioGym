using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class RolePermissionsRepositoryTests
{
    private AppDbContext? _context;
    private RolePermissionsRepository? _rolePermissionsRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new AppDbContext(options);
        _context.RolePermissions.RemoveRange(_context.RolePermissions);
        _context.SaveChanges();
        _rolePermissionsRepository = new RolePermissionsRepository(_context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void GetPermissions_WhenRoleExists_ReturnsAllItsPermissions()
    {
        var seeded = new RolePermissions
        {
            Role = Role.Client,
            Permissions = [Permission.PlaceOrder, Permission.GetMyOrders, Permission.GetProducts]
        };
        _context!.RolePermissions.Add(seeded);
        _context.SaveChanges();

        var result = _rolePermissionsRepository!.GetPermissions(Role.Client);

        CollectionAssert.AreEquivalent(seeded.Permissions, result);
    }

    [TestMethod]
    public void GetPermissions_WhenRoleHasNoPermissions_ReturnsEmpty()
    {
        var result = _rolePermissionsRepository!.GetPermissions(Role.Admin);

        Assert.AreEqual(0, result.Count);
    }
}
