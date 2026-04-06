using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public void CreateProduct(CreateProductDto newProduct)
    {
        if (newProduct.Name == null || newProduct.Name == string.Empty)
        {
            throw new BadRequestException("Name is required.");
        }

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
        Product product = _repository.GetById(updatedProduct.Id ?? 0);

        _repository.Update(product);
        _repository.Save();
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
