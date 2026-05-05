using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.OrderDTOs;
using DarkKitchen.Models.ProductDTOs;
using DarkKitchen.Models.UserDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class OrderServiceTest
{
    private Mock<IOrderRepository>? orderRepositoryMock;
    private Mock<IUserService>? userServiceMock;
    private Mock<IProductService>? productServiceMock;
    private OrderService? orderService;
    private OrderDto validOrder;
    private Order? orderEntity;
    private ProductDto productDto;

    [TestInitialize]
    public void Setup()
    {
        orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
        userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
        orderService = new OrderService(orderRepositoryMock.Object, userServiceMock.Object, productServiceMock.Object);

        productDto = new ProductDto(1, "PROD01", "Product 1", null, null, null, 100, null, true, 0);

        validOrder = new OrderDto("express", new AddressDto("18 de Julio", "1234", "101"), [new OrderProductDto("PROD01", 2)]);

        orderEntity = new Order()
        {
            Id = 1,
            ClientId = 1,
            DeliveryType = "express",
            Address = new Address { Street = "18 de Julio", DoorNumber = "1234" },
            CreatedAt = DateTime.Now,
            Subtotal = 200,
            ShippingCost = 10,
            Total = 256.2m
        };
    }

    [TestMethod]
    public void CreateOrder_ValidData_OrderCreated()
    {
        productServiceMock!.Setup(s => s.GetByCode("PROD01")).Returns(productDto);
        productServiceMock!.Setup(s => s.GetBestDiscountByProduct(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>()))
            .Returns([]);
        orderRepositoryMock!.Setup(r => r.Add(It.IsAny<Order>()));

        var result = orderService!.CreateOrder(validOrder, 1);

        orderRepositoryMock!.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void CreateOrder_WhenProductHasActivePromotion_DiscountApplied()
    {
        productServiceMock!.Setup(s => s.GetByCode("PROD01")).Returns(productDto);
        productServiceMock!.Setup(s => s.GetBestDiscountByProduct(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>()))
            .Returns(new Dictionary<int, int> { { 1, 25 } });
        orderRepositoryMock!.Setup(r => r.Add(It.IsAny<Order>()));

        var result = orderService!.CreateOrder(validOrder, 1);

        Assert.AreEqual(50, result.discount);
    }

    [TestMethod]
    public void CreateOrder_NoProducts_ThrowsBadRequestException()
    {
        validOrder = validOrder with { products = [] };

        Assert.ThrowsException<BadRequestException>(() => orderService!.CreateOrder(validOrder, 1));
    }

    [TestMethod]
    public void CreateOrder_InvalidDeliveryType_ThrowsBadRequestException()
    {
        validOrder = validOrder with { deliveryType = "invalid" };

        Assert.ThrowsException<BadRequestException>(() => orderService!.CreateOrder(validOrder, 1));
    }

    [TestMethod]
    public void CreateOrder_EmptyStreet_ThrowsBadRequestException()
    {
        validOrder = validOrder with { address = validOrder.address with { street = string.Empty } };

        Assert.ThrowsException<BadRequestException>(() => orderService!.CreateOrder(validOrder, 1));
    }

    [TestMethod]
    public void GetClientOrders_ValidData_ReturnsOrders()
    {
        orderRepositoryMock!.Setup(r => r.GetClientOrders(1, null, null, null))
            .Returns([orderEntity!]);
        var filters = new OrderFiltersDto { };

        var result = orderService!.GetClientOrders(1, filters);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetOrdersByStatus_ValidData_ReturnsOrders()
    {
        orderRepositoryMock!.Setup(r => r.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null))
            .Returns([orderEntity!]);
        var filters = new OrderFilterByStatusDto
        {
            DateFrom = DateTime.Now.AddDays(-7),
            DateTo = DateTime.Now
        };

        var result = orderService!.GetOrdersByStatus(filters);

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

        Assert.ThrowsException<NotFoundException>(() => orderService!.GetOrderById(1));
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
    public void CreateOrder_EmptyDoorNumber_ThrowsBadRequestException()
    {
        validOrder = validOrder with { address = validOrder.address with { doorNumber = string.Empty } };

        Assert.ThrowsException<BadRequestException>(() => orderService!.CreateOrder(validOrder, 1));
    }

    [TestMethod]
    public void CreateOrder_ProductNotFound_ThrowsNotFoundException()
    {
        productServiceMock!.Setup(s => s.GetByCode("PROD01")).Returns((ProductDto?)null);

        Assert.ThrowsException<NotFoundException>(() => orderService!.CreateOrder(validOrder, 1));
    }

    [TestMethod]
    public void CreateOrder_NextDayDelivery_OrderCreated()
    {
        validOrder = validOrder with { deliveryType = "24hs" };
        productServiceMock!.Setup(s => s.GetByCode("PROD01")).Returns(productDto);
        productServiceMock!.Setup(s => s.GetBestDiscountByProduct(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>()))
            .Returns([]);
        orderRepositoryMock!.Setup(r => r.Add(It.IsAny<Order>()));

        var result = orderService!.CreateOrder(validOrder, 1);

        Assert.IsNotNull(result);
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

    [TestMethod]
    public void UpdateOrderStatus_InvalidCurrentStatus_ThrowsBadRequestException()
    {
        orderEntity!.Status = "InvalidStatus";
        orderRepositoryMock!.Setup(r => r.GetById(1)).Returns(orderEntity!);

        Assert.ThrowsException<BadRequestException>(() =>
            orderService!.UpdateOrderStatus(1, new UpdateOrderStatusDto(nameof(OrderStatus.Prepared)), [Permission.SetOrderStatusToPrepared]));
    }

    [TestMethod]
    public void GetSalesReport_NoOrders_ReturnsEmptyReport()
    {
        orderRepositoryMock!.Setup(r => r.GetAll()).Returns([]);

        var result = orderService!.GetSalesReport();

        Assert.AreEqual(0, result.months.Count);
        Assert.AreEqual(0, result.total);
    }

    [TestMethod]
    public void GetSalesReport_ValidData_ReturnsReport()
    {
        var user = new UserResponseDto(1, "validName", "validSurname", "validEmail@gmail.com", "099123456", "Client");
        orderRepositoryMock!.Setup(r => r.GetAll()).Returns([orderEntity!]);
        userServiceMock!.Setup(s => s.GetUserById(1)).Returns(user);

        var result = orderService!.GetSalesReport();

        Assert.AreEqual(1, result.months.Count);
        Assert.AreEqual("validName validSurname", result.months[0].lines[0].clientName);
    }
}
