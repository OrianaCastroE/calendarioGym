using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
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
    }

    public void UpdatePromotionProducts(int promotionId, List<int> productIds)
    {
        var promotion = _promotionRepository.GetById(promotionId)
            ?? throw new Exception("Promotion not found.");

        _promotionRepository.SetProducts(promotionId, productIds);
    }

    public List<PromotionResponseDto> GetPromotions(PromotionFiltersDto filter)
    {
        var promotions = _promotionRepository.GetPromotions(filter.date, filter.productLine, filter.productName);

        return promotions.Select(p => new PromotionResponseDto(p.Id, p.Name, p.DiscountPercentage, p.DateFrom, p.DateTo)).ToList();
    }

    public Dictionary<int, int> GetBestDiscountByProduct(IEnumerable<int> productIds, DateTime date)
    {
        var requested = productIds.ToHashSet();
        var promotions = _promotionRepository.GetActiveForProducts(requested, date);
        var best = new Dictionary<int, int>();

        foreach(var promo in promotions)
        {
            foreach(var product in promo.Products.Where(p => requested.Contains(p.Id)))
            {
                if(!best.TryGetValue(product.Id, out var current) || promo.DiscountPercentage > current)
                {
                    best[product.Id] = promo.DiscountPercentage;
                }
            }
        }

        return best;
    }
}
