using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class ShippingTypeRepository(AppDbContext context) : IShippingTypeRepository
{
    public IEnumerable<ShippingType> GetAll()
    {
        return context.ShippingTypes.ToList();
    }

    public ShippingType? GetById(int id)
    {
        return context.ShippingTypes.Find(id);
    }

    public void Add(ShippingType shippingType)
    {
        context.ShippingTypes.Add(shippingType);
        context.SaveChanges();
    }

    public void Update(ShippingType shippingType)
    {
        throw new NotImplementedException();
    }
}
