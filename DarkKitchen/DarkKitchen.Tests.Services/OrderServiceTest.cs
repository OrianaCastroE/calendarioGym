using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
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
        orderRepositoryMock!.Setup(r => r.Add(It.IsAny<Order>()));

        var result = orderService!.CreateOrder(validOrder);

        orderRepositoryMock!.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
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
    public void UpdateOrderStatus_ToPrepared_StatusUpdated()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);
        orderRepositoryMock.Setup(r => r.Update(orderEntity!));

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Prepared)), [Permission.SetOrderStatusToPrepared]);

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ToCanceled_StatusUpdated()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);
        orderRepositoryMock.Setup(r => r.Update(orderEntity!));

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Canceled)), [Permission.SetOrderStatusToCanceled]);

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ToOnItsWay_StatusUpdated()
    {
        orderEntity!.Status = nameof(OrderStatus.Prepared);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);
        orderRepositoryMock.Setup(r => r.Update(orderEntity!));

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.OnItsWay)), [Permission.SetOrderStatusToOnItsWay]);

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ToDelivered_StatusUpdated()
    {
        orderEntity!.Status = nameof(OrderStatus.OnItsWay);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);
        orderRepositoryMock.Setup(r => r.Update(orderEntity!));

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Delivered)), [Permission.SetOrderStatusToDelivered]);

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_ToNotDelivered_StatusUpdated()
    {
        orderEntity!.Status = nameof(OrderStatus.OnItsWay);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);
        orderRepositoryMock.Setup(r => r.Update(orderEntity!));

        orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.NotDelivered)), [Permission.SetOrderStatusToNotDelivered]);

        orderRepositoryMock!.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void UpdateOrderStatus_InvalidStatus_ThrowsBadRequestException()
    {
        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto("NotAStatus"), [Permission.SetOrderStatusToPrepared]));
    }

    [TestMethod]
    public void UpdateOrderStatus_StatusWithoutMappedPermission_ThrowsBadRequestException()
    {
        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Pending)), [Permission.SetOrderStatusToPrepared]));
    }

    [TestMethod]
    public void UpdateOrderStatus_UserMissingRequiredPermission_ThrowsAccessDeniedException()
    {
        Assert.ThrowsException<AccessDeniedException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Prepared)), [Permission.SetOrderStatusToCanceled]));
    }

    [TestMethod]
    public void UpdateOrderStatus_OrderNotFound_ThrowsNotFoundException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns((Order?)null);

        Assert.ThrowsException<NotFoundException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Prepared)), [Permission.SetOrderStatusToPrepared]));
    }

    [TestMethod]
    public void CreateOrder_EmptyDoorNumber_ThrowsException()
    {
        validOrder = validOrder with { address = validOrder.address with { doorNumber = string.Empty } };

        Assert.ThrowsException<Exception>(() => orderService!.CreateOrder(validOrder));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromPendingToOnItsWay_ThrowsBadRequestException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.OnItsWay)), [Permission.SetOrderStatusToOnItsWay]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromPendingToDelivered_ThrowsBadRequestException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Delivered)), [Permission.SetOrderStatusToDelivered]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromPendingToNotDelivered_ThrowsBadRequestException()
    {
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.NotDelivered)), [Permission.SetOrderStatusToNotDelivered]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromCanceled_ThrowsBadRequestException()
    {
        orderEntity!.Status = nameof(OrderStatus.Canceled);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Prepared)), [Permission.SetOrderStatusToPrepared]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromDelivered_ThrowsBadRequestException()
    {
        orderEntity!.Status = nameof(OrderStatus.Delivered);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.OnItsWay)), [Permission.SetOrderStatusToOnItsWay]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromNotDelivered_ThrowsBadRequestException()
    {
        orderEntity!.Status = nameof(OrderStatus.NotDelivered);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.OnItsWay)), [Permission.SetOrderStatusToOnItsWay]));
    }

    [TestMethod]
    public void UpdateOrderStatus_FromPreparedToCanceled_ThrowsBadRequestException()
    {
        orderEntity!.Status = nameof(OrderStatus.Prepared);
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Canceled)), [Permission.SetOrderStatusToCanceled]));
    }
}
