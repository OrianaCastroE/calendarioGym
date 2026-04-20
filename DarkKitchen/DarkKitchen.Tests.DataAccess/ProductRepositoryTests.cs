using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class ProductRepositoryTests
{
    private Product? product;
    private ProductFilterDto? filter;
    private AppDbContext? context;
    private ProductRepository? productRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        product = new Product
        {
            Id = 1,
            Code = "TEST001",
            Name = "Test Product",
            Description = "This is a test product.",
            Category = "Test Category",
            Price = 9.99m,
            IsActive = true,
            Images =
            [
                new ProductImage
                {
                    Id = 1,
                    Url = "https://example.com/image1.jpg",
                    ProductId = 1
                },
                new ProductImage
                {
                    Id = 2,
                    Url = "https://example.com/image2.jpg",
                    ProductId = 1
                }

            ]
        };

        filter = new ProductFilterDto("productLine", ["cat1", "cat2"], "Product");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);
        productRepository = new ProductRepository(context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        context!.Database.EnsureDeleted();
        context.Dispose();
    }

    [TestMethod]
    public void AddProduct_WhenProductIsValid_AddsProductToDatabase()
    {
        productRepository!.Add(product!);
        context!.SaveChanges();

        var result = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == 1);

        Assert.IsNotNull(result);
        Assert.AreEqual("Test Product", result.Name);
        Assert.AreEqual(2, result.Images.Count);
    }

    [TestMethod]
    public void AddProduct_WhenProductIsNull_DoesNotAddProductToDatabase()
    {
        productRepository!.Add(null!);
        context!.SaveChanges();

        var result = context.Products.FirstOrDefault(p => p.Id == 1);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void UpdateProduct_WhenProductIsValid_ShouldUpdateProductInDatabase()
    {
        context!.Products.Add(product!);
        context!.SaveChanges();
        product!.Name = "Updated Product";

        productRepository!.Update(product!);
        context.SaveChanges();
        var result = context.Products.FirstOrDefault(p => p.Id == product.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Product", result.Name);
    }

    [TestMethod]
    public void UpdaProduct_WhenProductIsNull_ShouldNotUpdateProductInDatabase()
    {
        context!.Products.Add(product!);
        context!.SaveChanges();
        productRepository!.Update(null!);
        context.SaveChanges();

        var result = context.Products.FirstOrDefault(p => p.Id == product.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("Test Product", result.Name);
    }

    [TestMethod]
    public void GetById_WhenProductExists_ReturnsProduct()
    {
        context!.Products.Add(product!);
        context.SaveChanges();

        var result = productRepository!.GetById(product!.Id);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetById_WhenProductDoesNotExist_ReturnsNull()
    {
        var result = productRepository!.GetById(999);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetProducts_WhenProductsExist_ReturnsProducts()
    {
        context!.Products.Add(product!);
        context.SaveChanges();
        filter = new ProductFilterDto(null, null, "Test");
        var result = productRepository!.GetProducts(filter);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetProducts_WhenNoProductsExist_ReturnsEmptyList()
    {
        filter = new ProductFilterDto(null, null, "Test");
        var result = productRepository!.GetProducts(filter);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void GetProducts_WhenFilterIsNull_ReturnsAllProducts()
    {
        context!.Products.Add(product!);
        context.SaveChanges();
        var result = productRepository!.GetProducts(null);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void UpdateStatus_WhenProductIsValid_ShouldUpdateProductStatusInDatabase()
    {
        context!.Products.Add(product!);
        context.SaveChanges();

        productRepository!.UpdateStatus(product!.Id, new ProductStatusDto(false));
        context.SaveChanges();

        var result = context.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsActive);
    }

    [TestMethod]
    public void UpdateStatus_WhenProductDoesNotExist_ShouldNotUpdateProductStatusInDatabase()
    {
        context!.Products.Add(product!);
        context.SaveChanges();
        productRepository!.UpdateStatus(999, new ProductStatusDto(false));
        context.SaveChanges();
        var result = context.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsActive);
    }

    [TestMethod]
    public void GetMostRequestedProducts_WhenProductsExistInRange_ReturnsOrderedByQuantity()
    {
        context!.Products.Add(product!);
        var product2 = new Product { Id = 2, Code = "TEST002", Name = "Product 2", Price = 5.00m, IsActive = true };
        context.Products.Add(product2);

        var order = new Order
        {
            Id = 1,
            ClientId = 1,
            CreatedAt = new DateTime(2024, 6, 15),
            Products =
            [
                new OrderProduct { ProductId = 1, Quantity = 1, UnitPrice = 9.99m },
                new OrderProduct { ProductId = 2, Quantity = 5, UnitPrice = 5.00m }
            ]
        };
        context.Orders.Add(order);
        context.SaveChanges();

        var dates = new DateRangeDto(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
        var result = productRepository!.GetMostRequestedProducts(dates).ToList();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(2, result[0].Id);
        Assert.AreEqual(1, result[1].Id);
    }

    [TestMethod]
    public void GetMostRequestedProducts_WhenNoProductsInRange_ReturnsEmptyList()
    {
        var dates = new DateRangeDto(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
        var result = productRepository!.GetMostRequestedProducts(dates).ToList();
        Assert.AreEqual(0, result.Count);
    }
}
