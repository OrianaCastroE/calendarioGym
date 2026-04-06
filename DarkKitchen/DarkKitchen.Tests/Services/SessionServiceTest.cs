using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Models.SessionDTOs;
using DarkKitchen.Services;
using Moq;

namespace DarkKitchen.Tests.Services;

[TestClass]
public class SessionServiceTest
{
    private readonly string email = "valid@email.com";
    private readonly string password = "validPassword1!";
    private Mock<IUserRepository>? userRepositoryMock;
    private SessionService? sessionService;
    private User? user;
    private LoginDto? loginDto;

    [TestInitialize]
    public void Setup()
    {
        userRepositoryMock = new Mock<IUserRepository>();
        sessionService = new SessionService(userRepositoryMock);
        user = new User
        {
            Name = "Name",
            Surname = "Surname",
            Email = email,
            Phone = "+59899123123",
            Password = password,
            Role = Role.Client
        };
        loginDto = new LoginDto
        {
            Email = email,
            Password = password
        };
    }

    [TestMethod]
    public void Login_WithValidCredentials_ReturnsToken()
    {
        userRepositoryMock.Setup(r => r.GetByEmail(email)).Returns(user!);
        var result = sessionService!.Login(loginDto!);

        Assert.IsNotNull(result.Token);
    }

    [TestMethod]
    public void Login_WithInvalidCredentials_ThrowsUnauthorizedException()
    {
        userRepositoryMock.Setup(r => r.GetByEmail(email)).Throws(new UnauthorizedException("Invalid credentials."));

        Assert.ThrowsException<UnauthorizedException>(() => sessionService!.Login(loginDto!));
    }
}
