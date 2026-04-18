using DarkKitchen.DataAccess;
using DarkKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class UserRepositoryTests
{
    private readonly string validEmail = "valid@email.com";
    private User user;
    private AppDbContext _context;
    private UserRepository _userRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        user = new User
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = "+598099123456",
            Password = "Password1!",
            Role = Role.Client,
        };
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void GetUserByEmail_WhenUserExists_ReturnsUser()
    {
        _context.Users.Add(user);
        _context.SaveChanges();

        var result = _userRepository.GetUserByEmail(validEmail);

        Assert.IsNotNull(result);
        Assert.AreEqual(validEmail, result.Email);
    }
}
