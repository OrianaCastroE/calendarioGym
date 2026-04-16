using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Models.PromotionDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class PromotionServiceTest
{
    private Mock<IPromotionRepository>? promotionRepositoryMock;
    private PromotionService? promotionService;
    private PromotionDto validPromotion;
    private Promotion? promotionEntity;

    [TestInitialize]
    public void Setup()
    {
        promotionRepositoryMock = new Mock<IPromotionRepository>(MockBehavior.Strict);
        promotionService = new PromotionService(promotionRepositoryMock.Object);

        validPromotion = new PromotionDto("Black Friday", 10, new DateTime(2026, 1, 25), new DateTime(2026, 1, 30));

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
        promotionRepositoryMock!.Setup(r => r.Add(It.IsAny<Promotion>()));

        promotionService!.CreatePromotion(validPromotion);

        promotionRepositoryMock!.Verify(r => r.Add(It.IsAny<Promotion>()), Times.Once);
    }

    [TestMethod]
    public void CreatePromotion_InvalidDiscountPercentage_ThrowsException()
    {
        validPromotion = validPromotion with { discountPercentage = 0 };

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion));
    }

    [TestMethod]
    public void CreatePromotion_InvalidDateRange_ThrowsException()
    {
        validPromotion = validPromotion with { dateFrom = new DateTime(2026, 1, 30), dateTo = new DateTime(2026, 1, 25) };

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion));
    }

    [TestMethod]
    public void CreatePromotion_EmptyName_ThrowsException()
    {
        validPromotion = validPromotion with { name = string.Empty };

        Assert.ThrowsException<Exception>(() => promotionService!.CreatePromotion(validPromotion));
    }

    [TestMethod]
    public void UpdatePromotion_ValidData_PromotionUpdated()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns(promotionEntity!);
        promotionRepositoryMock!.Setup(r => r.Update(It.IsAny<Promotion>()));

        promotionService!.UpdatePromotion(1, validPromotion);

        promotionRepositoryMock!.Verify(r => r.Update(It.IsAny<Promotion>()), Times.Once);
    }

    [TestMethod]
    public void UpdatePromotion_PromotionNotFound_ThrowsException()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns((Promotion?)null);

        Assert.ThrowsException<Exception>(() => promotionService!.UpdatePromotion(1, validPromotion));
    }

    [TestMethod]
    public void UpdatePromotionProducts_ValidData_ProductsUpdated()
    {
        promotionRepositoryMock!.Setup(r => r.GetById(1)).Returns(promotionEntity!);
        promotionRepositoryMock!.Setup(r => r.Update(It.IsAny<Promotion>()));

        promotionService!.UpdatePromotionProducts(1, [1, 2, 3]);

        promotionRepositoryMock!.Verify(r => r.Update(It.IsAny<Promotion>()), Times.Once);
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

        var result = promotionService!.GetPromotions(new PromotionFiltersDto(null, null, null));

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetPromotions_NoPromotionsFound_ReturnsEmptyList()
    {
        promotionRepositoryMock!.Setup(r => r.GetPromotions(null, null, null))
            .Returns([]);

        var result = promotionService!.GetPromotions(new PromotionFiltersDto(null, null, null));

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}
