using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class ProductRepositoryTests
{
    private Product? product;
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
}
