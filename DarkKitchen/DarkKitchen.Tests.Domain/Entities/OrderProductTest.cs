using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Tests.Domain.Entities;

[TestClass]
public class OrderProductTest
{
    private readonly int id = 1;
    private readonly int orderId = 1;
    private readonly int productId = 1;
    private readonly int quantity = 2;
    private readonly decimal unitPrice = 100;
    private readonly decimal discountPercentage = 10;
    private OrderProduct? orderProduct;

    [TestInitialize]
    public void Setup()
    {
        orderProduct = new OrderProduct
        {
            Id = id,
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            DiscountPercentage = discountPercentage
        };
    }

    [TestMethod]
    public void OrderProduct_WithValidData_HasCorrectOrderId()
    {
        Assert.AreEqual(orderId, orderProduct!.OrderId);
    }

    [TestMethod]
    public void OrderProduct_WithValidData_HasCorrectProductId()
    {
        Assert.AreEqual(productId, orderProduct!.ProductId);
    }

    [TestMethod]
    public void OrderProduct_WithValidData_HasCorrectQuantity()
    {
        Assert.AreEqual(quantity, orderProduct!.Quantity);
    }

    [TestMethod]
    public void OrderProduct_WithValidData_HasCorrectUnitPrice()
    {
        Assert.AreEqual(unitPrice, orderProduct!.UnitPrice);
    }

    [TestMethod]
    public void OrderProduct_WithValidData_HasCorrectDiscountPercentage()
    {
        Assert.AreEqual(discountPercentage, orderProduct!.DiscountPercentage);
    }
}
