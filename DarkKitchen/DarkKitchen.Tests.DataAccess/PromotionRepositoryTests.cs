using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class PromotionRepositoryTests
{
    private Promotion? promotion;
    private AppDbContext? context;
    private PromotionRepository? promotionRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        promotion = new Promotion
        {
            Name = "Black Friday",
            DiscountPercentage = 10,
            DateFrom = new DateTime(2026, 1, 25),
            DateTo = new DateTime(2026, 1, 30)
        };

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);
        promotionRepository = new PromotionRepository(context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        context!.Database.EnsureDeleted();
        context.Dispose();
    }

    [TestMethod]
    public void AddPromotion_WhenPromotionIsValid_AddsPromotionToDatabase()
    {
        promotionRepository!.Add(promotion!);
        context!.SaveChanges();

        var result = context.Promotion.FirstOrDefault(p => p.Name == "Black Friday");

        Assert.IsNotNull(result);
        Assert.AreEqual("Black Friday", result.Name);
    }

    [TestMethod]
    public void GetById_WhenPromotionExists_ReturnsPromotion()
    {
        context!.Promotion.Add(promotion!);
        context.SaveChanges();

        var result = promotionRepository!.GetById(promotion!.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Black Friday", result.Name);
    }

    [TestMethod]
    public void GetById_WhenPromotionDoesNotExist_ReturnsNull()
    {
        var result = promotionRepository!.GetById(999);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void UpdatePromotion_WhenPromotionExists_UpdatesPromotionInDatabase()
    {
        context!.Promotion.Add(promotion!);
        context.SaveChanges();
        promotion!.Name = "Updated Promotion";

        promotionRepository!.Update(promotion!);
        context.SaveChanges();
        var result = context.Promotion.FirstOrDefault(p => p.Id == promotion.Id);

        Assert.AreEqual("Updated Promotion", result!.Name);
    }

    [TestMethod]
    public void UpdatePromotion_WhenPromotionDoesNotExist_ThrowsException()
    {
        Assert.ThrowsException<Exception>(() =>
        {
            promotionRepository!.Update(promotion!);
        });
    }

    [TestMethod]
    public void GetPromotions_WhenDateFilter_ReturnsMatchingPromotions()
    {
        context!.Promotion.Add(promotion!);
        context.SaveChanges();

        var result = promotionRepository!.GetPromotions(new DateTime(2026, 1, 27), null, null);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetPromotions_WhenNoFilter_ReturnsAllPromotions()
    {
        context!.Promotion.Add(promotion!);
        context.SaveChanges();

        var result = promotionRepository!.GetPromotions(null, null, null);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetPromotions_WhenNoPromotionsExist_ReturnsEmptyList()
    {
        var result = promotionRepository!.GetPromotions(null, null, null);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }
}
