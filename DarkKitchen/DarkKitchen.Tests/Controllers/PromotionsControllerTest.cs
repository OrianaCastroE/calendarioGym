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
}
