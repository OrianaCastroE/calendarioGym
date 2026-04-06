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
    private UpdateProductDto? validUpdateProductDto;

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

        validUpdateProductDto = new UpdateProductDto
        {
            Id = 1,
            Code = "PROD01",
            Name = "Updated Product",
            Description = "Updated Description",
            ProductLine = "Updated Line",
            Category = "Updated Category",
            Price = 150,
            IsActive = true,
            ImageUrl = ["http://example.com/updated_image.jpg"]
        };
    }

    [TestMethod]
    public void CreateProduct_WhenValidParams_CallsRepository()
    {
        productService!.CreateProduct(validCreateProductDto!);
        productRepositoryMock!.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    public void CreateProduct_WhenNameIsNull_ThrowsBadRequestException()
    {
        var dto = new CreateProductDto { Name = null };
        Assert.ThrowsException<BadRequestException>(() => productService!.CreateProduct(dto));
    }

    [TestMethod]
    public void CreateProduct_WhenNameIsEmpty_ThrowsBadRequestException()
    {
        var dto = new CreateProductDto
        {
            Name = string.Empty
        };
        Assert.ThrowsException<BadRequestException>(() => productService!.CreateProduct(dto));
    }

    [TestMethod]
    public void UpdateProduct_WhenProductExists_CallsRepository()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(validProduct!);
        productService!.UpdateProduct(validUpdateProductDto!);
        productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    public void UpdateProduct_WhenProductNotFound_ThrowsNotFoundException()
    {
        productRepositoryMock!.Setup(r => r.GetById(It.IsAny<int>())).Returns((Product?)null);
        Assert.ThrowsException<NotFoundException>(() => productService!.UpdateProduct(validUpdateProductDto!));
    }

    [TestMethod]
    public void GetProducts_WhenProductsExist_ReturnsMappedDtos()
    {
        productRepositoryMock!.Setup(r => r.GetProducts(It.IsAny<ProductFilter>())).Returns([validProduct!]);
        var result = productService!.GetProducts("Valid Line", null, null);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetProducts_WhenNoProductsFound_ThrowsNotFoundException()
    {
        productRepositoryMock!.Setup(r => r.GetProducts(It.IsAny<ProductFilter>())).Returns([]);
        Assert.ThrowsException<NotFoundException>(() => productService!.GetProducts(null, null, null));
    }
}
