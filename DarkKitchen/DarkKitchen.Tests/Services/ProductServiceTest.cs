using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;

// using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class ProductServiceTest
{
    private Mock<IProductRepository>? productRepositoryMock;
    private ProductService? productService;
    private Product? validProduct;
    private CreateProductDto? validCreateProductDto;

    [TestInitialize]
    public void Setup()
    {
        productRepositoryMock = new Mock<IProductRepository>();
        productService = new ProductService(productRepositoryMock.Object);

        validProduct = new Product
        {
            Id = 1,
            Code = "PROD01",
            Name = "Valid Product",
            Description = "Valid Description",
            ProductLine = "Valid Line",
            Category = "Valid Category",
            Price = 100,
            IsActive = true,
            Images = [new ProductImage { Id = 1, Url = "http://example.com/image.jpg", ProductId = 1 }]
        };

        validCreateProductDto = new CreateProductDto
        {
            Code = "PROD01",
            Name = "Valid Product",
            Description = "Valid Description",
            ProductLine = "Valid Line",
            Category = "Valid Category",
            Price = 100,
            ImageUrl = ["http://example.com/image.jpg"]
        };
    }

    [TestMethod]
    public void CreateProduct_WhenValidParams_CallsRepository()
    {
        productService!.CreateProduct(validCreateProductDto!);
        productRepositoryMock!.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
    }
}
