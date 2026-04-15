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
        if(string.IsNullOrEmpty(newPromotion.name))
        {
            throw new Exception("Name cannot be empty.");
        }

        if(newPromotion.discountPercentage <= 0 || newPromotion.discountPercentage > 100)
        {
            throw new Exception("Discount percentage must be between 1 and 100.");
        }

        if(newPromotion.dateFrom >= newPromotion.dateTo)
        {
            throw new Exception("DateFrom must be before DateTo.");
        }

        var promotion = new Promotion()
        {
            Name = newPromotion.name,
            DiscountPercentage = newPromotion.discountPercentage,
            DateFrom = newPromotion.dateFrom,
            DateTo = newPromotion.dateTo
        };

        _promotionRepository.Add(promotion);
        _promotionRepository.Save();
    }

    public void UpdatePromotion(int id, PromotionDto updatedPromotion)
    {
        var promotion = _promotionRepository.GetById(id)
            ?? throw new Exception("Promotion not found.");

        promotion.Name = updatedPromotion.name;
        promotion.DiscountPercentage = updatedPromotion.discountPercentage;
        promotion.DateFrom = updatedPromotion.dateFrom;
        promotion.DateTo = updatedPromotion.dateTo;

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

    public List<PromotionResponseDto> GetPromotions(PromotionFiltersDto filter)
    {
        var promotions = _promotionRepository.GetPromotions(filter.date, filter.productLine, filter.productName);

        return promotions.Select(p => new PromotionResponseDto(p.Id, p.Name, p.DiscountPercentage, p.DateFrom, p.DateTo)).ToList();
    }
}
