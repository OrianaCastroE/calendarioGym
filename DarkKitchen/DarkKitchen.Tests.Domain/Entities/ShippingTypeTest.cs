using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Tests.Domain.Entities;

[TestClass]
public class ShippingTypeTest
{
    private readonly string validName = "Express";
    private readonly decimal validPrice = 250;

    [TestMethod]
    public void ShippingType_WithValidData_CreatedCorrectly()
    {
        var shippingType = new ShippingType(validName, validPrice);

        Assert.AreEqual(validName, shippingType.Name);
        Assert.AreEqual(validPrice, shippingType.Price);
    }
}
