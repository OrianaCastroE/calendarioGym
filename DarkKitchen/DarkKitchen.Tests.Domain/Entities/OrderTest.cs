using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Tests.Domain.Entities;

[TestClass]
public class OrderTest
{
    private readonly int id = 1;
    private readonly int clientId = 1;
    private readonly string deliveryType = "express";
    private readonly string street = "18 de Julio";
    private readonly string doorNumber = "1234";
    private readonly string apartment = "101";
    private readonly string status = "Pending";
    private readonly DateTime createdAt = new DateTime(2026, 1, 25);
    private Order? order;

    [TestInitialize]
    public void Setup()
    {
        order = new Order
        {
            Id = id,
            ClientId = clientId,
            DeliveryType = deliveryType,
            Street = street,
            DoorNumber = doorNumber,
            Apartment = apartment,
            Status = status,
            CreatedAt = createdAt
        };
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectClientId()
    {
        Assert.AreEqual(clientId, order!.ClientId);
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectDeliveryType()
    {
        Assert.AreEqual(deliveryType, order!.DeliveryType);
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectStreet()
    {
        Assert.AreEqual(street, order!.Street);
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectDoorNumber()
    {
        Assert.AreEqual(doorNumber, order!.DoorNumber);
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectStatus()
    {
        Assert.AreEqual(status, order!.Status);
    }

    [TestMethod]
    public void Order_WithValidData_HasCorrectCreatedAt()
    {
        Assert.AreEqual(createdAt, order!.CreatedAt);
    }

    [TestMethod]
    public void Order_UpdatedAtIsNullByDefault()
    {
        Assert.IsNull(order!.UpdatedAt);
    }
}
