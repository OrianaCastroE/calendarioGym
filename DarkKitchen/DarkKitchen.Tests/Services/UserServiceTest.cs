using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Models.UserDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class UserServiceTest
{
    private Mock<IUserRepository>? _userRepositoryMock;
    private UserService? _userService;
    private UserDto? _validUser;

    [TestInitialize]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);

        _validUser = new UserDto
        {
            Name = "validName",
            Surname = "validSurname",
            Email = "validEmail@gmail.com",
            Phone = "099123456",
            Password = "validPassword123!",
        };
    }

    [TestMethod]
    public void CreateUser_WhenValidUser_ShouldCreateUser()
    {
        _userRepositoryMock!
            .Setup(repository => repository.Add(It.IsAny<User>()))
            .Returns((User user) => user);

        _userService!.CreateUser(_validUser!);

        _userRepositoryMock!
            .Verify(repository => repository.Add(It.IsAny<User>()), Times.Once);
    }

    [TestMethod]
    public void CreateUser_WhenInvalidName_ShouldNotCreateUser()
    {
        _validUser!.Name = string.Empty;
        if (string.IsNullOrEmpty(_validUser.Name))
        {
            throw new ArgumentException("Name cannot be empty or whitespace.");
        }

        _userService!.CreateUser(_validUser!);

        _userRepositoryMock!
            .Verify(repository => repository.Add(It.IsAny<User>()), Times.Never);
    }
}
