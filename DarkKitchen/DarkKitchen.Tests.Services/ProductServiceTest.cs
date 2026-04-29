using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Models.DateDTOs;
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
    private CreateProductDto validCreateProductDto;
    private ProductDto validProductDto;

    [TestInitialize]
    public void Setup()
    {
        productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
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

        validCreateProductDto = new CreateProductDto("PROD01", "Valid Product", "Valid Description", "Valid Line", "Valid Category", 100, ["http://example.com/image.jpg"]);

        validProductDto = new ProductDto(1, "PROD01", "Updated Product", "Updated Description", "Updated Line", "Updated Category", 150, ["http://example.com/updated_image.jpg"], true, 0);
    }

    [TestMethod]
    public void CreateProduct_WhenValidParams_CallsRepository()
    {
        productRepositoryMock!.Setup(r => r.Add(It.IsAny<Product>()));

        productService!.CreateProduct(validCreateProductDto);

        productRepositoryMock!.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    public void CreateProduct_WhenNameIsNull_ThrowsBadRequestException()
    {
        var dto = new CreateProductDto(null, null, null, null, null, null, null);

        Assert.ThrowsException<BadRequestException>(() => productService!.CreateProduct(dto));
    }

    [TestMethod]
    public void CreateProduct_WhenNameIsEmpty_ThrowsBadRequestException()
    {
        var dto = new CreateProductDto(null, string.Empty, null, null, null, null, null);

        Assert.ThrowsException<BadRequestException>(() => productService!.CreateProduct(dto));
    }

    [TestMethod]
    public void UpdateProduct_WhenProductExists_CallsRepository()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(validProduct!);
        productRepositoryMock!.Setup(r => r.Update(It.IsAny<Product>()));

        productService!.UpdateProduct(validProductDto);

        productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    public void UpdateProduct_WhenProductNotFound_ThrowsNotFoundException()
    {
        productRepositoryMock!.Setup(r => r.GetById(It.IsAny<int>())).Returns((Product?)null);

        Assert.ThrowsException<NotFoundException>(() => productService!.UpdateProduct(validProductDto));
    }

    [TestMethod]
    public void GetProducts_WhenProductsExist_ReturnsMappedDtos()
    {
        productRepositoryMock!.Setup(r => r.GetProducts(It.IsAny<ProductFilterDto>())).Returns([validProduct!]);

        var result = productService!.GetProducts(new ProductFilterDto("Valid Line", null, null));

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetProducts_WhenNoProductsFound_ThrowsNotFoundException()
    {
        productRepositoryMock!.Setup(r => r.GetProducts(It.IsAny<ProductFilterDto>())).Returns([]);

        Assert.ThrowsException<NotFoundException>(() => productService!.GetProducts(new ProductFilterDto(null, null, null)));
    }

    [TestMethod]
    public void GetProducts_WhenProductHasNoImages_ReturnsMappedDtoWithNullImages()
    {
        validProduct!.Images = null;
        productRepositoryMock!.Setup(r => r.GetProducts(It.IsAny<ProductFilterDto>())).Returns([validProduct!]);

        var result = productService!.GetProducts(new ProductFilterDto(null, null, null));

        Assert.IsNull(result.Single().imageUrl);
    }

    [TestMethod]
    public void GetMostRequestedProducts_WhenProductsExist_ReturnsMappedDtos()
    {
        var dates = new DateRangeDto(DateTime.Now.AddDays(-7), DateTime.Now);
        productRepositoryMock!.Setup(r => r.GetMostRequestedProducts(dates)).Returns([validProduct!]);

        var result = productService!.GetMostRequestedProducts(dates);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetMostRequestedProducts_WhenNoProductsFound_ThrowsNotFoundException()
    {
        var dates = new DateRangeDto(DateTime.Now.AddDays(-7), DateTime.Now);
        productRepositoryMock!.Setup(r => r.GetMostRequestedProducts(dates)).Returns([]);

        Assert.ThrowsException<NotFoundException>(() => productService!.GetMostRequestedProducts(dates));
    }

    [TestMethod]
    public void GetMostRequestedProducts_WhenProductHasNoImages_ReturnsMappedDtoWithNullImages()
    {
        var dates = new DateRangeDto(DateTime.Now.AddDays(-7), DateTime.Now);
        validProduct!.Images = null;
        productRepositoryMock!.Setup(r => r.GetMostRequestedProducts(dates)).Returns([validProduct!]);

        var result = productService!.GetMostRequestedProducts(dates);

        Assert.IsNull(result.Single().imageUrl);
    }

    [TestMethod]
    public void UpdateStatus_WhenProductExists_CallsRepository()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(validProduct!);
        productRepositoryMock!.Setup(r => r.Update(It.IsAny<Product>()));
        var status = new ProductStatusDto(false);

        productService!.UpdateStatus(1, status);

        productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    public void UpdateStatus_WhenProductNotFound_ThrowsNotFoundException()
    {
        productRepositoryMock!.Setup(r => r.GetById(It.IsAny<int>())).Returns((Product?)null);
        var status = new ProductStatusDto(false);

        Assert.ThrowsException<NotFoundException>(() => productService!.UpdateStatus(1, status));
    }

    [TestMethod]
    public void UpdateProduct_WhenFieldsAreNull_DoesNotUpdate()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(validProduct!);
        productRepositoryMock!.Setup(r => r.Update(It.IsAny<Product>()));

        var dtoWithNulls = new ProductDto(1, null, null, null, null, null, null, null, null, null);

        productService!.UpdateProduct(dtoWithNulls);

        productRepositoryMock.Verify(r => r.Update(It.Is<Product>(p =>
            p.Name == validProduct!.Name &&
            p.Description == validProduct!.Description &&
            p.ProductLine == validProduct!.ProductLine &&
            p.Category == validProduct!.Category &&
            p.Price == validProduct!.Price)),
            Times.Once);
    }

    [TestMethod]
    public void GetByCode_WhenProductExists_ReturnsDto()
    {
        productRepositoryMock!.Setup(r => r.GetByCode("PROD01")).Returns(validProduct!);

        var result = productService!.GetByCode("PROD01");

        Assert.IsNotNull(result);
        Assert.AreEqual("PROD01", result.Value.code);
    }

    [TestMethod]
    public void GetByCode_WhenProductDoesNotExist_ReturnsNull()
    {
        productRepositoryMock!.Setup(r => r.GetByCode("MISSING")).Returns((Product?)null);

        var result = productService!.GetByCode("MISSING");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void UpdateProduct_WhenOnlyNameIsProvided_UpdatesOnlyName()
    {
        productRepositoryMock!.Setup(r => r.GetById(1)).Returns(validProduct!);
        productRepositoryMock!.Setup(r => r.Update(It.IsAny<Product>()));

        var dtoOnlyName = new ProductDto(1, null, "New Name", null, null, null, null, null, null, null);

        productService!.UpdateProduct(dtoOnlyName);

        productRepositoryMock.Verify(r => r.Update(It.Is<Product>(p =>
            p.Name == "New Name" &&
            p.Description == validProduct!.Description &&
            p.ProductLine == validProduct!.ProductLine &&
            p.Category == validProduct!.Category &&
            p.Price == validProduct!.Price)),
            Times.Once);
    }
}
