using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Domain.Interfaces.Service;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(ProductDto updatedProduct);
    public IEnumerable<ProductDto> GetProducts(ProductFilterDto filter);
    public ProductDto? GetByCode(string code);
    public IEnumerable<ProductDto> GetMostRequestedProducts(DateRangeDto dates);
    public void UpdateStatus(int id, ProductStatusDto status);
}
