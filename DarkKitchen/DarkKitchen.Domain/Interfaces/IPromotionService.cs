using DarkKitchen.Models.PromotionDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IPromotionService
{
    public void CreatePromotion(PromotionDto newPromotion);
    public void UpdatePromotion(int id, PromotionDto updatedPromotion);
    public void UpdatePromotionProducts(int promotionId, List<int> productIds);
    public List<PromotionResponseDto> GetPromotions(PromotionFiltersDto filter);
}
