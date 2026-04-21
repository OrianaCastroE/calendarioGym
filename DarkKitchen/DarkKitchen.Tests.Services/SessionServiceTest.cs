using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Models.SessionDTOs;
using DarkKitchen.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class SessionServiceTest
{
    private readonly string email = "valid@email.com";
    private readonly string password = "validPassword1!";
    private Mock<IUserRepository>? userRepositoryMock;
    private Mock<IConfiguration>? configurationMock;
    private SessionService? sessionService;
    private User? user;
    private LoginDto loginDto;

    [TestInitialize]
    public void Setup()
    {
        userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);
        configurationMock.Setup(c => c["Jwt:Key"]).Returns("test-secret-key-minimum-32-chars!!");
        sessionService = new SessionService(userRepositoryMock.Object, configurationMock.Object);
        user = new User
        {
            Name = "Name",
            Surname = "Surname",
            Email = email,
            Phone = "+59899123123",
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = Role.Client
        };
        loginDto = new LoginDto(email, password);
    }

    [TestMethod]
    public void Login_WithValidCredentials_ReturnsToken()
    {
        userRepositoryMock!.Setup(r => r.GetByEmail(email)).Returns(user!);

        var result = sessionService!.Login(loginDto);

        Assert.IsNotNull(result.token);
    }

    [TestMethod]
    public void Login_WithWrongPassword_ThrowsUnauthorizedException()
    {
        userRepositoryMock!.Setup(r => r.GetByEmail(email)).Returns(user!);
        var wrongPasswordDto = new LoginDto(email, "wrongPassword1!");

        Assert.ThrowsException<UnauthorizedException>(() => sessionService!.Login(wrongPasswordDto));
    }

    [TestMethod]
    public void Login_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        userRepositoryMock!.Setup(r => r.GetByEmail(email)).Returns((User?)null);

        Assert.ThrowsException<UnauthorizedException>(() => sessionService!.Login(loginDto));
    }
}
