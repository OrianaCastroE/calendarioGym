using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;
namespace DarkKitchen.DataAccess.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    private readonly AppDbContext context = context;

    public void Add(Product product)
    {
        return;
    }

    public void Update(Product product)
    {
        return;
    }

    public Product? GetById(int id)
    {
        return null;
    }

    public IEnumerable<Product> GetProducts(ProductFilterDto filter)
    {
        return null;
    }

    public IEnumerable<Product> GetMostRequestedProducts(DateRangeDto dates)
    {
        return null;
    }

    public void UpdateStatus(int id, ProductStatusDto status)
    {
        return;
    }
}
