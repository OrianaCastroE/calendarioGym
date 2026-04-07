using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.PromotionDTOs;

namespace DarkKitchen.Services;

public class PromotionService(IPromotionRepository promotionRepository) : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;

    public void CreatePromotion(PromotionDto newPromotion)
    {
        if(string.IsNullOrEmpty(newPromotion.Name))
        {
            throw new Exception("Name cannot be empty.");
        }

        if(newPromotion.DiscountPercentage <= 0 || newPromotion.DiscountPercentage > 100)
        {
            throw new Exception("Discount percentage must be between 1 and 100.");
        }

        if(newPromotion.DateFrom >= newPromotion.DateTo)
        {
            throw new Exception("DateFrom must be before DateTo.");
        }

        var promotion = new Promotion()
        {
            Name = newPromotion.Name,
            DiscountPercentage = newPromotion.DiscountPercentage,
            DateFrom = newPromotion.DateFrom,
            DateTo = newPromotion.DateTo
        };

        _promotionRepository.Add(promotion);
        _promotionRepository.Save();
    }

    public void UpdatePromotion(int id, PromotionDto updatedPromotion)
    {
        var promotion = _promotionRepository.GetById(id)
            ?? throw new Exception("Promotion not found.");

        promotion.Name = updatedPromotion.Name;
        promotion.DiscountPercentage = updatedPromotion.DiscountPercentage;
        promotion.DateFrom = updatedPromotion.DateFrom;
        promotion.DateTo = updatedPromotion.DateTo;

        _promotionRepository.Update(promotion);
        _promotionRepository.Save();
    }

    public void UpdatePromotionProducts(int promotionId, List<int> productIds)
    {
        var promotion = _promotionRepository.GetById(promotionId)
            ?? throw new Exception("Promotion not found.");

        promotion.Products = promotion.Products
            .Where(p => productIds.Contains(p.Id))
            .ToList();

        _promotionRepository.Update(promotion);
        _promotionRepository.Save();
    }

    public List<PromotionResponseDto> GetPromotions(DateTime? date, string? productLine, string? productName)
    {
        throw new NotImplementedException();
    }
}
