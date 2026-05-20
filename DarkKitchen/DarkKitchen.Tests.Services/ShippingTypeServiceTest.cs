using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Models.ShippingTypeDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class ShippingTypeServiceTest
{
    private Mock<IShippingTypeRepository>? shippingTypeRepositoryMock;
    private ShippingTypeService? shippingTypeService;
    private ShippingTypeDto validDto;
    private ShippingType? shippingTypeEntity;

    [TestInitialize]
    public void Setup()
    {
        shippingTypeRepositoryMock = new Mock<IShippingTypeRepository>(MockBehavior.Strict);
        shippingTypeService = new ShippingTypeService(shippingTypeRepositoryMock.Object);

        validDto = new ShippingTypeDto("Express", 250);
        shippingTypeEntity = new ShippingType { Id = 1, Name = "Express", Price = 250 };
    }

    [TestMethod]
    public void Create_WithValidData_CallsRepositoryAndReturnsDto()
    {
        shippingTypeRepositoryMock!.Setup(r => r.Add(It.IsAny<ShippingType>()));

        var result = shippingTypeService!.Create(validDto);

        shippingTypeRepositoryMock!.Verify(r => r.Add(It.IsAny<ShippingType>()), Times.Once);
        Assert.AreEqual(validDto.name, result.name);
        Assert.AreEqual(validDto.price, result.price);
    }
}
