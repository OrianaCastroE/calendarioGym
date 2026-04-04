using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(ProductDto updatedProduct);
    public IEnumerable<ProductDto> GetProducts(string? productLine, List<string>? categories, string? name);
    public IEnumerable<ProductDto> GetMostRequestedProducts();
    public void UpdateStatus(int id, ProductStatusDto status);
}
