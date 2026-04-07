using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Domain.DataAccess.Interfaces;

public interface IPromotionRepository
{
    void Add(Promotion promotion);
    void Update(Promotion promotion);
    Promotion? GetById(int id);
    IEnumerable<Promotion> GetPromotions(DateTime? date, string? productLine, string? productName);
    void Save();
}
