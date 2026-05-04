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
        context.SaveChanges();
    }

    public void Update(Product product)
    {
        if(product == null)
        {
            return;
        }

        context.Products.Update(product);
        context.SaveChanges();
    }

    public Product? GetById(int id)
    {
        return context.Products.FirstOrDefault(p => p.Id == id);
    }

    public Product? GetByCode(string code)
    {
        return context.Products.FirstOrDefault(p => p.Code == code);
    }

    public IEnumerable<Product> GetProducts(ProductFilterDto filter)
    {
        var query = context.Products.AsQueryable();

        if(!string.IsNullOrEmpty(filter.productLine))
        {
            query = query.Where(p => p.ProductLine == filter.productLine);
        }

        if(filter.categories != null && filter.categories.Count > 0)
        {
            query = query.Where(p => p.Category != null && filter.categories.Contains(p.Category));
        }

        if(!string.IsNullOrEmpty(filter.name))
        {
            query = query.Where(p => p.Name.Contains(filter.name));
        }

        return query.ToList();
    }

    public IEnumerable<Product> GetMostRequestedProducts(DateRangeDto dates)
    {
        var productIds = context.Orders
            .Where(o => o.CreatedAt >= dates.dateFrom && o.CreatedAt <= dates.dateTo)
            .SelectMany(o => o.Products)
            .GroupBy(op => op.ProductId)
            .OrderByDescending(g => g.Sum(op => op.Quantity))
            .Select(g => g.Key)
            .ToList();

        return context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToList()
            .OrderBy(p => productIds.IndexOf(p.Id));
    }

    public void UpdateStatus(int id, ProductStatusDto status)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);
        if(product == null)
        {
            return;
        }

        product.IsActive = status.isActive;
        context.Products.Update(product);
        context.SaveChanges();
    }
}
