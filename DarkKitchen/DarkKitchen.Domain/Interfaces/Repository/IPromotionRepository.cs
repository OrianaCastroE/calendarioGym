using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Domain.Interfaces.Repository;

public interface IPromotionRepository
{
    void Add(Promotion promotion);
    void Update(Promotion promotion);
    void SetProducts(int promotionId, List<int> productIds);
    Promotion? GetById(int id);
    IEnumerable<Promotion> GetPromotions(DateTime? date, string? productLine, string? productName);
    IEnumerable<Promotion> GetActiveForProducts(IEnumerable<int> productIds, DateTime date);
}
