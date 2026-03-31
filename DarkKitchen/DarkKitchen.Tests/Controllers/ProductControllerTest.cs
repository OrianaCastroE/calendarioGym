using DarkKitchen.API.Controllers;
using Domain.DTOs.ProductDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

// using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
public class ProductControllerTest
{
    private readonly Guid validProductId = Guid.NewGuid();
    private readonly string validProductName = "Valid Product Name";
    private readonly string validProductDescription = "Valid Product Description";
    private readonly string validProductLine = "Valid Product Line";
    private readonly string validCategory = "Valid Category";
    private readonly string[] validImageUrl = ["http://example.com/image.jpg"];
    private ProductDto? validProduct;
    private CreateProductDto? validCreateProduct;
    private ProductsController? productController;
    private Mock<IProductService>? productServiceMock;

    [TestInitialize]
    public void Setup()
    {
        productServiceMock = new Mock<IProductService>();
        productController = new ProductsController(productServiceMock.Object);

        validProduct = new ProductDto()
        {
            Id = validProductId,
            Name = validProductName,
            Description = validProductDescription,
            Category = validCategory,
            ImageUrl = validImageUrl
        };

        validCreateProduct = new CreateProductDto()
        {
            Name = validProductName,
            Description = validProductDescription,
            Category = validCategory,
            ImageUrl = validImageUrl
        };
    }

    [TestMethod]
    public void CreateProduct_WhenValidParams_ShouldCreateProduct()
    {
        var result = productController.CreateProduct(validCreateProduct!);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj!.StatusCode);
    }

    [TestMethod]
    public void CreateProduct_WhenNameIsNull_ShouldntCreateProduct()
    {
        var nullProductName = new CreateProductDto()
        {
            Name = null,
            Description = validProductDescription,
            Category = validCategory,
            ImageUrl = validImageUrl
        };

        productServiceMock.Setup(s => s.CreateProduct(nullProductName)).Throws(new Exception("Invalid name."));
        var result = productController.CreateProduct(nullProductName);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj!.StatusCode);
    }

    [TestMethod]
    public void GetProducts_WhenValidProducts_ShouldReturnProducts()
    {
        List<string> categories = ["Fútbol", "Baloncesto", "Tenis"];
        var products = new List<ProductDto>();
        productServiceMock.Setup(s => s.GetProducts(validProductLine, categories, validProductName)).Returns(products!);
        var result = productController.GetProducts(validProductLine, categories, validProductName);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj!.StatusCode);
        Assert.AreEqual(products, resultObj.Value);
    }

    [TestMethod]
    public void GetProducts_WhenNoProducts_ShouldReturnEmptyList()
    {
        List<string> categories = [];
        productServiceMock.Setup(s => s.GetProducts(validProductLine, categories, validProductName)).Throws(new Exception("No products found."));
        var result = productController.GetProducts(validProductLine, categories, validProductName);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }
}
