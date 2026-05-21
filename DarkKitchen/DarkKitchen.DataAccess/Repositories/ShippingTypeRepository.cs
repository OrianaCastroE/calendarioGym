using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class ShippingTypeRepository(AppDbContext context) : IShippingTypeRepository
{
    public IEnumerable<ShippingType> GetAll()
    {
        throw new NotImplementedException();
    }

    public ShippingType? GetById(int id)
    {
        throw new NotImplementedException();
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
