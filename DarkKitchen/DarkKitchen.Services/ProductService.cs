using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public void CreateProduct(CreateProductDto newProduct)
    {
        var product = new Product
        {
            Code = newProduct.Code!,
            Name = newProduct.Name!,
            Description = newProduct.Description,
            ProductLine = newProduct.ProductLine,
            Category = newProduct.Category,
            Price = newProduct.Price!.Value,
            IsActive = true,
            Images = newProduct.ImageUrl!.Select(url => new ProductImage { Url = url }).ToList()
        };
        _repository.Add(product);
    }

    public void UpdateProduct(UpdateProductDto updatedProduct)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UpdateProductDto> GetProducts(string? productLine, List<string>? categories, string? name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UpdateProductDto> GetMostRequestedProducts(DateRangeDto dates)
    {
        throw new NotImplementedException();
    }

    public void UpdateStatus(int id, ProductStatusDto status)
    {
        throw new NotImplementedException();
    }
}
