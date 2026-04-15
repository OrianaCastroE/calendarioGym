using DarkKitchen.API.Controllers;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.SessionDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.API.Controllers;

[TestClass]
public class SessionsControllerTest
{
    private readonly string validEmail = "valid@email.com";
    private readonly string password = "ValidPassword123!";
    private Mock<ISessionService>? sessionServiceMock;
    private SessionsController? sessionController;

    [TestInitialize]
    public void Setup()
    {
        sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
        sessionController = new SessionsController(sessionServiceMock.Object);
    }

    [TestMethod]
    public void Login_ValidCredentials_ReturnsOkWithToken()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Returns(new LoginResponseDto("fake-jwt-token"));

        var loginDto = new LoginDto(validEmail, password);
        var result = sessionController.Login(loginDto);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void Login_InvalidCredentials_ThrowsBadRequestException()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new BadRequestException("Invalid credentials."));

        var loginDto = new LoginDto(validEmail, "wrongpassword");

        Assert.ThrowsException<BadRequestException>(() => sessionController.Login(loginDto));
    }

    [TestMethod]
    public void Login_NullEmail_ThrowsBadRequestException()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new BadRequestException("Email is required."));

        var loginDto = new LoginDto(null, password);

        Assert.ThrowsException<BadRequestException>(() => sessionController.Login(loginDto));
    }

    [TestMethod]
    public void Login_NullPassword_ThrowsBadRequestException()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new BadRequestException("Password is required."));

        var loginDto = new LoginDto(validEmail, null);

        Assert.ThrowsException<BadRequestException>(() => sessionController.Login(loginDto));
    }

    [TestMethod]
    public void Login_InvalidEmailFormat_ThrowsBadRequestException()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new BadRequestException("Invalid credentials."));

        var loginDto = new LoginDto("notanemail", password);

        Assert.ThrowsException<BadRequestException>(() => sessionController.Login(loginDto));
    }
}
