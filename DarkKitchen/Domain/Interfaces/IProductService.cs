using Models.ProductDTOs;

namespace Domain.Interfaces;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(ProductDto updatedProduct);
    public IEnumerable<ProductDto> GetProducts(string? productLine, List<string>? categories, string? name);
    public IEnumerable<ProductDto> GetMostRequestedProducts();
}
