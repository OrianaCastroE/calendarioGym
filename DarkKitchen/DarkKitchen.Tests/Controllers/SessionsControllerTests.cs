using DarkKitchen.API.Controllers;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.SessionDTOs;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
public class SessionsControllerTests
{
    private readonly string validEmail = "valid@email.com";
    private readonly string password = "ValidPassword123!";
    private Mock<ISessionService>? sessionServiceMock;
    private SessionsController? sessionController;

    [TestInitialize]
    public void Setup()
    {
        sessionServiceMock = new Mock<ISessionService>();
        sessionController = new SessionsController(sessionServiceMock.Object);
    }

    [TestMethod]
    public void Login_ValidCredentials_ReturnsOkWithToken()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Returns(new LoginResponseDto { Token = "fake-jwt-token" });

        var loginDto = new LoginDto { Email = validEmail, Password = password };
        var result = sessionController.Login(loginDto);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void Login_InvalidCredentials_ReturnsBadRequest()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new Exception("Invalid credentials."));

        var loginDto = new LoginDto { Email = validEmail, Password = "wrongpassword" };
        var result = sessionController.Login(loginDto);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void Login_NullEmail_ReturnsBadRequest()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new Exception("Email is required."));

        var loginDto = new LoginDto { Email = null, Password = password };
        var result = sessionController.Login(loginDto);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void Login_NullPassword_ReturnsBadRequest()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new Exception("Password is required."));

        var loginDto = new LoginDto { Email = validEmail, Password = null };
        var result = sessionController.Login(loginDto);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void Login_InvalidEmailFormat_ReturnsBadRequest()
    {
        sessionServiceMock.Setup(s => s.Login(It.IsAny<LoginDto>())).Throws(new Exception("Invalid credentials."));

        var loginDto = new LoginDto { Email = "notanemail", Password = password };
        var result = sessionController.Login(loginDto);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }
}
