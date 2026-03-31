using DarkKitchen.API.Controllers;
using Domain.DTOs.ProductDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

// using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
internal class ProductControllerTest
{
    private readonly string validProductId = "1";
    private readonly string validProductName = "Valid Product Name";
    private readonly string validProductDescription = "Valid Product Description";
    private readonly string validCategory = "Valid Category";
    private readonly string validImageUrl = "http://example.com/image.jpg";
    private ProductDto? validProduct;
    private ProductsController? productController;


    private Mock<IProductService>? productServiceMock;

    [TestInitialize]
    public void Setup()
    {
        productServiceMock = new Mock<IProductService>();
        productController = new ProductsController(productServiceMock.Object);

        validProduct = new ProductDto()
        {
            Id = Guid.Parse(validProductId),
            Name = validProductName,
            Description = validProductDescription,
            Category = validCategory,
            ImageUrl = validImageUrl
        };
    }

    [TestMethod]
    public void CreateProduct_WhenValidParams_ShouldCreateProduct()
    {
        productServiceMock.Setup(s => s.CreateProduct(validProduct!));
        var result = productController.CreateProduct(validProduct);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj!.StatusCode);
    }

}
