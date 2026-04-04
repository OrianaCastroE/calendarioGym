using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(UpdateProductDto updatedProduct);
    public IEnumerable<UpdateProductDto> GetProducts(string? productLine, List<string>? categories, string? name);
    public IEnumerable<UpdateProductDto> GetMostRequestedProducts(DateRangeDto dates);
    public void UpdateStatus(int id, ProductStatusDto status);
}
