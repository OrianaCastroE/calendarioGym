using DarkKitchen.Domain.Entities;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Domain.Interfaces.Repository;

public interface IProductRepository
{
    void Add(Product product);
    void Update(Product product);
    Product? GetById(int id);
    Product? GetByCode(string code);
    IEnumerable<Product> GetProducts(ProductFilterDto filter);
    IEnumerable<Product> GetMostRequestedProducts(DateRangeDto dates);
    void UpdateStatus(int id, ProductStatusDto status);
}
