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

    [TestMethod]
    public void GetById_WhenOrderExists_ReturnsOrder()
    {
        context!.Orders.Add(order!);
        context.SaveChanges();

        var result = orderRepository!.GetById(order!.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Pending", result.Status);
    }

    [TestMethod]
    public void GetById_WhenOrderDoesNotExist_ReturnsNull()
    {
        var result = orderRepository!.GetById(999);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void UpdateOrder_WhenOrderExists_UpdatesOrderInDatabase()
    {
        context!.Orders.Add(order!);
        context.SaveChanges();
        order!.Status = "Prepared";

        orderRepository!.Update(order!);
        context.SaveChanges();
        var result = context.Orders.FirstOrDefault(o => o.Id == order.Id);

        Assert.AreEqual("Prepared", result!.Status);
    }

    [TestMethod]
    public void UpdateOrder_WhenOrderDoesNotExist_ThrowsException()
    {
        Assert.ThrowsException<Exception>(() =>
        {
            orderRepository!.Update(order!);
        });
    }

    [TestMethod]
    public void GetClientOrders_WhenOrdersExist_ReturnsOrders()
    {
        context!.Orders.Add(order!);
        context.SaveChanges();

        var result = orderRepository!.GetClientOrders(1, null, null, null);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetClientOrders_WhenNoOrdersExist_ReturnsEmptyList()
    {
        var result = orderRepository!.GetClientOrders(1, null, null, null);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void GetOrdersByStatus_WhenOrdersExist_ReturnsOrders()
    {
        context!.Orders.Add(order!);
        context.SaveChanges();

        var result = orderRepository!.GetOrdersByStatus(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), null, null);

        Assert.AreEqual(1, result.Count());
    }
}
