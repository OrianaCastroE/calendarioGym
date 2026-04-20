using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class OrderRepositoryTests
{
    private Order? order;
    private AppDbContext? context;
    private OrderRepository? orderRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        order = new Order
        {
            ClientId = 1,
            DeliveryType = "express",
            Street = "18 de Julio",
            DoorNumber = "1234",
            Apartment = "101",
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);
        orderRepository = new OrderRepository(context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        context!.Database.EnsureDeleted();
        context.Dispose();
    }

    [TestMethod]
    public void AddOrder_WhenOrderIsValid_AddsOrderToDatabase()
    {
        orderRepository!.Add(order!);
        context!.SaveChanges();

        var result = context.Orders.FirstOrDefault(o => o.ClientId == 1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Pending", result.Status);
    }
}
