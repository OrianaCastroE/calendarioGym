using DarkKitchen.API.Controllers;
using Domain.DTOs.OrderDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

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
            ClientId = 1,
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
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    public void CreateOrder_NoProducts_ReturnsBadRequest()
    {
        var emptyOrder = new OrderDto()
        {
            ClientId = 1,
            DeliveryType = "express",
            Address = new AddressDto()
            {
                Street = "18 de Julio",
                DoorNumber = "1234",
                Apartment = "101"
            },
            Products = []
        };
        orderServiceMock!.Setup(s => s.CreateOrder(emptyOrder))
            .Throws(new Exception("Order must have at least one product."));
        var result = ordersController!.CreateOrder(emptyOrder);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetClientOrders_ValidFilter_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetClientOrders(1, null, null, null)).Returns(orders!);
        var result = ordersController!.GetClientOrders(1, null, null, null);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetClientOrders_NoOrdersFound_ReturnsNotFound()
    {
        orderServiceMock!.Setup(s => s.GetClientOrders(1, null, null, null))
            .Throws(new Exception("No orders found."));
        var result = ordersController!.GetClientOrders(1, null, null, null);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrdersByStatus_ValidFilter_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null)).Returns(orders!);
        var result = ordersController!.GetOrdersByStatus(DateTime.Now.AddDays(-7), DateTime.Now, null, null);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrdersByStatus_NoOrdersFound_ReturnsNotFound()
    {
        orderServiceMock!.Setup(s => s.GetOrdersByStatus(It.IsAny<DateTime>(), It.IsAny<DateTime>(), null, null))
            .Throws(new Exception("No orders found."));
        var result = ordersController!.GetOrdersByStatus(DateTime.Now.AddDays(-7), DateTime.Now, null, null);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrderById_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.GetOrderById(1)).Returns(orderResponse!);
        var result = ordersController!.GetOrderById(1);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetOrderById_OrderNotFound_ReturnsNotFound()
    {
        orderServiceMock!.Setup(s => s.GetOrderById(1))
            .Throws(new Exception("Order not found."));
        var result = ordersController!.GetOrderById(1);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void PrepareOrder_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.PrepareOrder(1));
        var result = ordersController!.PrepareOrder(1);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void PrepareOrder_OrderNotPending_ReturnsBadRequest()
    {
        orderServiceMock!.Setup(s => s.PrepareOrder(1))
            .Throws(new Exception("Order is not pending."));
        var result = ordersController!.PrepareOrder(1);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void CancelOrder_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.CancelOrder(1));
        var result = ordersController!.CancelOrder(1);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void CancelOrder_OrderNotPending_ReturnsBadRequest()
    {
        orderServiceMock!.Setup(s => s.CancelOrder(1))
            .Throws(new Exception("Order is not pending."));
        var result = ordersController!.CancelOrder(1);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void SetOrderOnTheWay_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.SetOrderOnTheWay(1));
        var result = ordersController!.SetOrderOnTheWay(1);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void SetOrderOnTheWay_OrderNotPrepared_ReturnsBadRequest()
    {
        orderServiceMock!.Setup(s => s.SetOrderOnTheWay(1))
            .Throws(new Exception("Order is not prepared."));
        var result = ordersController!.SetOrderOnTheWay(1);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void DeliverOrder_ValidId_ReturnsOk()
    {
        orderServiceMock!.Setup(s => s.DeliverOrder(1));
        var result = ordersController!.DeliverOrder(1);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void DeliverOrder_OrderNotOnTheWay_ReturnsBadRequest()
    {
        orderServiceMock!.Setup(s => s.DeliverOrder(1))
            .Throws(new Exception("Order is not on the way."));
        var result = ordersController!.DeliverOrder(1);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }
}
