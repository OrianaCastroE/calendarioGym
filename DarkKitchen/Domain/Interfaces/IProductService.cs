using Domain.DTOs.ProductDTOs;

namespace Domain.Interfaces;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(ProductDto updatedProduct);
    public void GetProducts(string? productLine, List<string>? categories, string? name);
}
