using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IProductService
{
    public void CreateProduct(CreateProductDto newProduct);
    public void UpdateProduct(UpdateProductDto updatedProduct);
    public IEnumerable<UpdateProductDto> GetProducts(ProductFilterDto filter);
    public IEnumerable<UpdateProductDto> GetMostRequestedProducts(DateRangeDto dates);
    public void UpdateStatus(int id, ProductStatusDto status);
}
