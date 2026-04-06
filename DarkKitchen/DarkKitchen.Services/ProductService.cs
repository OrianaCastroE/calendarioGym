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
        Product product = _repository.GetById(updatedProduct.Id ?? 0)
        ?? throw new NotFoundException("Product not found.");

        if(updatedProduct.Name != null)
        {
            product.Name = updatedProduct.Name;
        }

        if(updatedProduct.Description != null)
        {
            product.Description = updatedProduct.Description;
        }

        if(updatedProduct.ProductLine != null)
        {
            product.ProductLine = updatedProduct.ProductLine;
        }

        if(updatedProduct.Category != null)
        {
            product.Category = updatedProduct.Category;
        }

        if(updatedProduct.Price.HasValue)
        {
            product.Price = updatedProduct.Price.Value;
        }

        if(updatedProduct.IsActive.HasValue)
        {
            product.IsActive = updatedProduct.IsActive.Value;
        }

        if(updatedProduct.ImageUrl != null)
        {
            product.Images = updatedProduct.ImageUrl.Select(url => new ProductImage { Url = url, ProductId = product.Id }).ToList();
        }

        _repository.Update(product);
        _repository.Save();
    }

    public IEnumerable<UpdateProductDto> GetProducts(string? productLine, List<string>? categories, string? name)
    {
        var filter = new ProductFilter
        {
            ProductLine = productLine,
            Categories = categories,
            Name = name
        };

        IEnumerable<Product> products = _repository.GetProducts(filter);

        if(!products.Any())
        {
            throw new NotFoundException("No products found.");
        }

        return _repository.GetProducts(filter).Select(p => new UpdateProductDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            ProductLine = p.ProductLine,
            Category = p.Category,
            Price = p.Price,
            IsActive = p.IsActive,
            ImageUrl = p.Images?.Select(i => i.Url).ToArray()
        });
    }

    public IEnumerable<UpdateProductDto> GetMostRequestedProducts(DateRangeDto dates)
    {
        IEnumerable<Product> products = _repository.GetMostRequestedProducts(dates);

        if(!products.Any())
        {
            throw new NotFoundException("No products found.");
        }

        return products.Select(p => new UpdateProductDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            ProductLine = p.ProductLine,
            Category = p.Category,
            Price = p.Price,
            IsActive = p.IsActive,
            ImageUrl = p.Images?.Select(i => i.Url).ToArray()
        });
    }

    public void UpdateStatus(int id, ProductStatusDto status)
    {
        Product product = _repository.GetById(id);

        product.IsActive = status.IsActive;
        _repository.Update(product);
        _repository.Save();
    }
}
