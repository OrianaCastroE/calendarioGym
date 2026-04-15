using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class OrderServiceTest
{
    private Mock<IOrderRepository>? orderRepositoryMock;
    private Mock<IProductRepository>? productRepositoryMock;
    private Mock<IPromotionRepository>? promotionRepositoryMock;
    private OrderService? orderService;
    private OrderDto validOrder;
    private Order? orderEntity;
    private Product? productEntity;

    [TestInitialize]
    public void Setup()
    {
        orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
        productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
        promotionRepositoryMock = new Mock<IPromotionRepository>(MockBehavior.Strict);
        orderService = new OrderService(orderRepositoryMock.Object);

        productEntity = new Product()
        {
            Id = 1,
            Code = "PROD01",
            Name = "Product 1",
            Price = 100,
            IsActive = true
        };

        validOrder = new OrderDto("express", new AddressDto("18 de Julio", "1234", "101"), [new OrderProductDto("PROD01", 2)]);

        orderEntity = new Order()
        {
            Id = 1,
            ClientId = 1,
            Status = "Pending",
            CreatedAt = DateTime.Now,
            Subtotal = 200,
            ShippingCost = 10,
            Total = 256.2m
        };
    }

    [TestMethod]
    public void CreateOrder_ValidData_OrderCreated()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(productEntity!);
        promotionRepositoryMock!.Setup(r => r.GetPromotions(null, null, null)).Returns([]);

        var result = orderService!.CreateOrder(validOrder);

        orderRepositoryMock!.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        orderRepositoryMock!.Verify(r => r.Save(), Times.Once);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void CreateOrder_NoProducts_ThrowsException()
    {
        validOrder = validOrder with { products = [] };

        Assert.ThrowsException<Exception>(() => orderService!.CreateOrder(validOrder));
    }

    [TestMethod]
    public void CreateOrder_InvalidDeliveryType_ThrowsException()
    {
        validOrder = validOrder with { deliveryType = "invalid" };

        Assert.ThrowsException<Exception>(() => orderService!.CreateOrder(validOrder));
    }

    [TestMethod]
    public void CreateOrder_EmptyStreet_ThrowsException()
    {
        validOrder = validOrder with { address = validOrder.address with { street = string.Empty } };

        Assert.ThrowsException<Exception>(() => orderService!.CreateOrder(validOrder));
    }

    [TestMethod]
    public void GetClientOrders_ValidData_ReturnsOrders()
    {
        orderRepositoryMock!.Setup(r => r.GetClientOrders(1, null, null, null))
            .Returns([orderEntity!]);

        var result = orderService!.GetClientOrders(1, new OrderFiltersDto(null, null, null));

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetOrdersByStatus_ValidData_ReturnsOrders()
    {
        orderRepositoryMock!.Setup(r => r.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null))
            .Returns([orderEntity!]);

        var result = orderService!.GetOrdersByStatus(new OrderFilterByStatusDto(DateTime.Now.AddDays(-7), DateTime.Now, null, null));

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetOrderById_ValidId_ReturnsOrder()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        var result = orderService!.GetOrderById(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.id);
    }

    [TestMethod]
    public void GetOrderById_OrderNotFound_ThrowsException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns((Order?)null);

        Assert.ThrowsException<Exception>(() => orderService!.GetOrderById(1));
    }

    [TestMethod]
    public void UpdateOrderStatus_ValidData_StatusUpdated()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto("Prepared"));

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
        orderRepositoryMock!.Verify(r => r.Save(), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_OrderNotFound_ThrowsException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns((Order?)null);

        Assert.ThrowsException<Exception>(() => orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto("Prepared")));
    }
}
