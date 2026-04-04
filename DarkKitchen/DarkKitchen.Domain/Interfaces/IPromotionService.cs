using DarkKitchen.Models.PromotionDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IPromotionService
{
    public void CreatePromotion(PromotionDto newPromotion);
    public void UpdatePromotion(PromotionDto updatedPromotion);
    public void AssociateProduct(int promotionId, string productCode);
    public void DisassociateProduct(int promotionId, string productCode);
    public List<PromotionResponseDto> GetPromotions(DateTime? date, string? productLine, string? productName);
}
