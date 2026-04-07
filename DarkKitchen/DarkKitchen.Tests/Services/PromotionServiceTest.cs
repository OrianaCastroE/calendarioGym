using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Models.PromotionDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class PromotionServiceTest
{
    private Mock<IPromotionRepository>? promotionRepositoryMock;
    private PromotionService? promotionService;
    private PromotionDto? validPromotion;
    private Promotion? promotionEntity;

    [TestInitialize]
    public void Setup()
    {
        promotionRepositoryMock = new Mock<IPromotionRepository>();
        promotionService = new PromotionService(promotionRepositoryMock.Object);

        validPromotion = new PromotionDto()
        {
            Name = "Black Friday",
            DiscountPercentage = 10,
            DateFrom = new DateTime(2026, 1, 25),
            DateTo = new DateTime(2026, 1, 30)
        };

        promotionEntity = new Promotion()
        {
            Id = 1,
            Name = "Black Friday",
            DiscountPercentage = 10,
            DateFrom = new DateTime(2026, 1, 25),
            DateTo = new DateTime(2026, 1, 30)
        };
    }

    [TestMethod]
    public void CreatePromotion_ValidData_PromotionCreated()
    {
        promotionService!.CreatePromotion(validPromotion!);

        promotionRepositoryMock!.Verify(r => r.Add(It.IsAny<Promotion>()), Times.Once);
        promotionRepositoryMock!.Verify(r => r.Save(), Times.Once);
    }

    [TestMethod]
    public void CreatePromotion_InvalidDiscountPercentage_ThrowsException()
    {
        validPromotion!.DiscountPercentage = 0;

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion!));
    }

    [TestMethod]
    public void CreatePromotion_InvalidDateRange_ThrowsException()
    {
        validPromotion!.DateFrom = new DateTime(2026, 1, 30);
        validPromotion!.DateTo = new DateTime(2026, 1, 25);

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion!));
    }

    [TestMethod]
    public void CreatePromotion_EmptyName_ThrowsException()
    {
        validPromotion!.Name = string.Empty;

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion!));
    }

    [TestMethod]
    public void UpdatePromotion_ValidData_PromotionUpdated()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns(promotionEntity!);

        promotionService!.UpdatePromotion(1, validPromotion!);

        promotionRepositoryMock!.Verify(r => r.Update(It.IsAny<Promotion>()), Times.Once);
        promotionRepositoryMock!.Verify(r => r.Save(), Times.Once);
    }

    [TestMethod]
    public void UpdatePromotion_PromotionNotFound_ThrowsException()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns((Promotion?)null);

        Assert.ThrowsException<Exception>(() => promotionService!.UpdatePromotion(1, validPromotion!));
    }

    [TestMethod]
    public void UpdatePromotionProducts_ValidData_ProductsUpdated()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns(promotionEntity!);

        promotionService!.UpdatePromotionProducts(1, [1, 2, 3]);

        promotionRepositoryMock!.Verify(r => r.Update(It.IsAny<Promotion>()), Times.Once);
        promotionRepositoryMock!.Verify(r => r.Save(), Times.Once);
    }

    [TestMethod]
    public void UpdatePromotionProducts_PromotionNotFound_ThrowsException()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns((Promotion?)null);

        Assert.ThrowsException<Exception>(() => promotionService!.UpdatePromotionProducts(1, [1, 2, 3]));
    }

    [TestMethod]
    public void GetPromotions_ValidFilter_ReturnsPromotions()
    {
        promotionRepositoryMock!.Setup(r => r.GetPromotions(null, null, null))
            .Returns([promotionEntity!]);

        var result = promotionService!.GetPromotions(null, null, null);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
    }
}
