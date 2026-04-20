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
        if(product == null)
        {
            return;
        }

        context.Products.Add(product);
    }

    public void Update(Product product)
    {
        if (product == null)
        {
            return;
        }

        context.Products.Update(product);
    }

    public Product? GetById(int id)
    {
        return context.Products.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Product> GetProducts(ProductFilterDto? filter)
    {
        var query = context.Products.AsQueryable();

        if (filter.HasValue)
        {
            var f = filter.Value;

            if (!string.IsNullOrEmpty(f.productLine))
            {
                query = query.Where(p => p.ProductLine == f.productLine);
            }

            if (f.categories != null && f.categories.Count > 0)
            {
                query = query.Where(p => p.Category != null && f.categories.Contains(p.Category));
            }

            if (!string.IsNullOrEmpty(f.name))
            {
                query = query.Where(p => p.Name.Contains(f.name));
            }
        }

        return query.ToList();
    }

    public IEnumerable<Product> GetMostRequestedProducts(DateRangeDto dates)
    {
        return null;
    }

    public void UpdateStatus(int id, ProductStatusDto status)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return;
        }

        product.IsActive = status.isActive;
        context.Products.Update(product);
    }
}
