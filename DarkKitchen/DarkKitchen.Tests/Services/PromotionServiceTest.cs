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
}
