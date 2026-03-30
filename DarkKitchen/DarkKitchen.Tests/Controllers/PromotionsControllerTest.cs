using DarkKitchen.API.Controllers;
using Domain.DTOs.PromotionDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
public class PromotionsControllerTest
{
    private Mock<IPromotionService>? promotionServiceMock;
    private PromotionsController? promotionsController;
    private PromotionDto? validPromotion;

    [TestInitialize]
    public void Setup()
    {
        promotionServiceMock = new Mock<IPromotionService>();
        promotionsController = new PromotionsController(promotionServiceMock.Object);

        validPromotion = new PromotionDto()
        {
            Name = "Black Friday",
            DiscountPercentage = 10,
            DateFrom = new DateTime(2026, 1, 25),
            DateTo = new DateTime(2026, 1, 30)
        };
    }

    [TestMethod]
    public void CreatePromotion_ValidData_ReturnsCreated()
    {
        promotionServiceMock!.Setup(s => s.CreatePromotion(validPromotion!));
        var result = promotionsController!.CreatePromotion(validPromotion!);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    public void CreatePromotion_AlreadyExists_ReturnsBadRequest()
    {
        promotionServiceMock!.Setup(s => s.CreatePromotion(validPromotion!))
            .Throws(new Exception("Promotion already exists."));
        var result = promotionsController!.CreatePromotion(validPromotion!);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void UpdatePromotion_ExistingPromotion_ReturnsOk()
    {
        promotionServiceMock!.Setup(s => s.UpdatePromotion(validPromotion!));
        var result = promotionsController!.UpdatePromotion(validPromotion!);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void UpdatePromotion_PromotionNotFound_ReturnsNotFound()
    {
        promotionServiceMock!.Setup(s => s.UpdatePromotion(validPromotion!))
            .Throws(new Exception("Promotion not found."));
        var result = promotionsController!.UpdatePromotion(validPromotion!);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }
}
