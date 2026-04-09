using DarkKitchen.API.Controllers;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.API.Controllers;

[TestClass]
public class OrdersControllerTest
{
    private Mock<IOrderService>? orderServiceMock;
    private OrdersController? ordersController;
    private OrderDto? validOrder;
    private OrderResponseDto? orderResponse;
    private List<OrderResponseDto>? orders;

    [TestInitialize]
    public void Setup()
    {
        orderServiceMock = new Mock<IOrderService>();
        ordersController = new OrdersController(orderServiceMock.Object);

        validOrder = new OrderDto()
        {
            DeliveryType = "express",
            Address = new AddressDto()
            {
                Street = "18 de Julio",
                DoorNumber = "1234",
                Apartment = "101"
            },
            Products =
            [
                new OrderProductDto() { ProductCode = "PROD01", Quantity = 2 }
            ]
        };

        orderResponse = new OrderResponseDto()
        {
            Id = 1,
            ClientId = 1,
            Status = "Pending",
            CreatedAt = DateTime.Now,
            Subtotal = 100,
            ShippingCost = 10,
            Total = 122
        };

        orders = [orderResponse];
    }

    [TestMethod]
    public void CreateOrder_ValidData_ReturnsCreated()
    {
        orderServiceMock!.Setup(s => s.CreateOrder(validOrder!)).Returns(orderResponse!);
        var result = ordersController!.CreateOrder(validOrder!);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    public void CreateOrder_NoProducts_ThrowsBadRequestException()
    {
        var emptyOrder = new OrderDto()
        {
            DeliveryType = "express",
            Address = new AddressDto()
            {
                Street = "18 de Julio",
                DoorNumber = "1234",
                Apartment = "101"
            },
            Products = []
        };
        orderServiceMock!.Setup(s => s.CreateOrder(emptyOrder)).Throws(new BadRequestException("Order must have at least one product."));

        Assert.ThrowsException<BadRequestException>(() => ordersController!.CreateOrder(emptyOrder));
    }

    [TestMethod]
    public void GetClientOrders_ValidFilter_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetClientOrders(It.IsAny<int>(), null, null, null)).Returns(orders!);
        var result = ordersController!.GetClientOrders(null, null, null);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetClientOrders_NoOrdersFound_ThrowsNotFoundException()
    {
        orderServiceMock!.Setup(s => s.GetClientOrders(It.IsAny<int>(), null, null, null)).Throws(new NotFoundException("No orders found."));

        Assert.ThrowsException<NotFoundException>(() => ordersController!.GetClientOrders(null, null, null));
    }

    [TestMethod]
    public void GetOrdersByStatus_ValidFilter_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null)).Returns(orders!);
        var result = ordersController!.GetOrdersByStatus(DateTime.Now.AddDays(-7), DateTime.Now, null, null);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrdersByStatus_NoOrdersFound_ThrowsNotFoundException()
    {
        orderServiceMock!.Setup(s => s.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null)).Throws(new NotFoundException("No orders found."));

        Assert.ThrowsException<NotFoundException>(() => ordersController!.GetOrdersByStatus(DateTime.Now.AddDays(-7), DateTime.Now, null, null));
    }

    [TestMethod]
    public void GetOrderById_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetOrderById(1)).Returns(orderResponse!);
        var result = ordersController!.GetOrderById(1);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrderById_OrderNotFound_ThrowsNotFoundException()
    {
        orderServiceMock!.Setup(s => s.GetOrderById(1)).Throws(new NotFoundException("Order not found."));

        Assert.ThrowsException<NotFoundException>(() => ordersController!.GetOrderById(1));
    }

    [TestMethod]
    public void UpdateOrderStatus_ValidStatus_ReturnsOk()
    {
        var newStatus = new UpdateOrderStatusDto { Status = "Preparing" };
        orderServiceMock!.Setup(s => s.UpdateOrderStatus(1, newStatus));
        var result = ordersController!.UpdateOrderStatus(1, newStatus);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void UpdateOrderStatus_InvalidStatus_ThrowsBadRequestException()
    {
        var newStatus = new UpdateOrderStatusDto { Status = "InvalidStatus" };
        orderServiceMock!.Setup(s => s.UpdateOrderStatus(1, newStatus)).Throws(new BadRequestException("Invalid order status."));

        Assert.ThrowsException<BadRequestException>(() => ordersController!.UpdateOrderStatus(1, newStatus));
    }

    [TestMethod]
    public void UpdateOrderStatus_OrderNotFound_ThrowsNotFoundException()
    {
        var newStatus = new UpdateOrderStatusDto { Status = "Preparing" };
        orderServiceMock!.Setup(s => s.UpdateOrderStatus(1, newStatus)).Throws(new NotFoundException("Order not found."));

        Assert.ThrowsException<NotFoundException>(() => ordersController!.UpdateOrderStatus(1, newStatus));
    }
}
