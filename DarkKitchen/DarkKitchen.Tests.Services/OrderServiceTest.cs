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
    private OrderDto? validOrder;
    private Order? orderEntity;
    private Product? productEntity;

    [TestInitialize]
    public void Setup()
    {
        orderRepositoryMock = new Mock<IOrderRepository>();
        productRepositoryMock = new Mock<IProductRepository>();
        promotionRepositoryMock = new Mock<IPromotionRepository>();
        orderService = new OrderService(
            orderRepositoryMock.Object,
            productRepositoryMock.Object,
            promotionRepositoryMock.Object
        );

        productEntity = new Product()
        {
            Id = 1,
            Code = "PROD01",
            Name = "Product 1",
            Price = 100,
            IsActive = true
        };

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

        var result = orderService!.CreateOrder(validOrder!);

        orderRepositoryMock!.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        orderRepositoryMock!.Verify(r => r.Save(), Times.Once);
        Assert.IsNotNull(result);
    }
}
