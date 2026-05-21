using DarkKitchen.API.Controllers;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.ShippingTypeDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.API.Controllers;

[TestClass]
public class ShippingTypesControllerTest
{
    private Mock<IShippingTypeService>? shippingTypeServiceMock;
    private ShippingTypesController? shippingTypesController;
    private ShippingTypeDto validDto;
    private ShippingTypeResponseDto shippingTypeResponse;
    private List<ShippingTypeResponseDto>? shippingTypes;

    [TestInitialize]
    public void Setup()
    {
        shippingTypeServiceMock = new Mock<IShippingTypeService>(MockBehavior.Strict);
        shippingTypesController = new ShippingTypesController(shippingTypeServiceMock.Object);

        validDto = new ShippingTypeDto("Express", 250);
        shippingTypeResponse = new ShippingTypeResponseDto(1, "Express", 250);
        shippingTypes = [shippingTypeResponse];
    }

    [TestMethod]
    public void GetAll_ReturnsOk()
    {
        shippingTypeServiceMock!.Setup(s => s.GetAll()).Returns(shippingTypes!);

        var result = shippingTypesController!.GetAll();
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }
}
