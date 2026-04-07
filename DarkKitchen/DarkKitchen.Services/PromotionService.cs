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
        if(newPromotion.DiscountPercentage <= 0 || newPromotion.DiscountPercentage > 100)
        {
            throw new Exception("Discount percentage must be between 1 and 100.");
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
        throw new NotImplementedException();
    }

    public void UpdatePromotionProducts(int promotionId, List<int> productIds)
    {
        throw new NotImplementedException();
    }

    public List<PromotionResponseDto> GetPromotions(DateTime? date, string? productLine, string? productName)
    {
        throw new NotImplementedException();
    }
}
