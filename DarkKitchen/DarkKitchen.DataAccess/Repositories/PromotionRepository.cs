using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class PromotionRepository(AppDbContext context) : IPromotionRepository
{
    public void Add(Promotion promotion)
    {
        context.Promotion.Add(promotion);
        context.SaveChanges();
    }

    public void Update(Promotion promotion)
    {
        var existingPromotion = context.Promotion.Find(promotion.Id);
        if(existingPromotion == null)
        {
            throw new Exception("Promotion not found");
        }

        context.Entry(existingPromotion).CurrentValues.SetValues(promotion);
        context.SaveChanges();
    }

    public Promotion? GetById(int id)
    {
        return context.Promotion.Find(id);
    }

    public IEnumerable<Promotion> GetPromotions(DateTime? date, string? productLine, string? productName)
    {
        var query = context.Promotion.AsQueryable();

        if(date.HasValue)
        {
            query = query.Where(p => p.DateFrom <= date && p.DateTo >= date);
        }

        return query.ToList();
    }
}
