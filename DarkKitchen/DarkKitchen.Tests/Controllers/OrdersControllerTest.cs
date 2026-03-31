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
}
