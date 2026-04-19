using DarkKitchen.DataAccess;
using DarkKitchen.DataAccess.Repositories;
using DarkKitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DarkKitchen.Tests.DataAccess;

[TestClass]
public class UserRepositoryTests
{
    private readonly string validEmail = "valid@email.com";
    private readonly string validEmail2 = "valid@email2.com";
    private User? user;
    private User? user2;
    private AppDbContext? _context;
    private UserRepository? _userRepository;

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
            Role = Role.Client
        };
        user2 = new User
        {
            Name = "Name2",
            Surname = "Surname2",
            Email = validEmail2,
            Phone = "+598099123456",
            Password = "Password1!",
            Role = Role.Client
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
        _context.Users.Add(user!);
        _context.SaveChanges();

        var result = _userRepository.GetByEmail(validEmail);

        Assert.AreEqual(validEmail, result.Email);
    }

    [TestMethod]
    public void GetUserByEmail_WhenUserDoesNotExist_ReturnsNull()
    {
        var result = _userRepository.GetByEmail(validEmail);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void AddUser_WhenUserIsValid_AddsUserToDatabase()
    {
        _userRepository.Add(user!);
        _context.SaveChanges();

        var result = _context.Users.FirstOrDefault(u => u.Email == validEmail);

        Assert.AreEqual(validEmail, result.Email);
    }

    [TestMethod]
    public void AddUser_WhenUserAlreadyExists_ThrowsException()
    {
        _context.Users.Add(user!);
        _context.SaveChanges();

        Assert.ThrowsException<ArgumentException>(() =>
        {
            _userRepository.Add(user!);
            _context.SaveChanges();
        });
    }

    [TestMethod]
    public void DeleteUser_WhenUserExists_DeletesUserFromDatabase()
    {
        _context.Users.Add(user!);
        _context.SaveChanges();

        _userRepository.Delete(user!);
        _context.SaveChanges();
        var result = _context.Users.FirstOrDefault(u => u.Email == validEmail);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void DeleteUser_WhenUserDoesNotExist_ThrowsException()
    {
        Assert.ThrowsException<DbUpdateConcurrencyException>(() =>
        {
            _userRepository.Delete(user!);
            _context.SaveChanges();
        });
    }

    [TestMethod]
    public void GetUsers_WhenUsersExist_ReturnsListOfUsers()
    {
        _context.Users.Add(user!);
        _context.SaveChanges();

        var result = _userRepository.GetUsers(user.Name, user.Surname);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(validEmail, result[0].Email);
    }

    [TestMethod]
    public void GetUsers_WhenNullParameters_ReturnsAllUsers()
    {
        _context.Users.Add(user!);
        _context.Users.Add(user2!);
        _context.SaveChanges();

        var result = _userRepository.GetUsers(null, null);

        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void GetUsers_WhenNoUsersExist_ReturnsEmptyList()
    {
        var result = _userRepository.GetUsers(null, null);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}
