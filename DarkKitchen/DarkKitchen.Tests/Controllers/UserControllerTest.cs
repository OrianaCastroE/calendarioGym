using DarkKitchen.API.Controllers;
using Domain.DTOs.UserDTO;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
public class UserControllerTest
{
    private readonly string validEmail = "valid@email.com";
    private readonly string password = "ValidPassword123!";
    private readonly string validPhone = "+59899123123";
    private Mock<IUserService>? userServiceMock;
    private UserController? userController;

    [TestInitialize]
    public void Setup()
    {
        userServiceMock = new Mock<IUserService>();
        userController = new UserController(userServiceMock.Object);
    }

    [TestMethod]
    public void UserSignUp_ValidUserWithoutRole_UserRegisteredAsClient()
    {
        userServiceMock.Setup(s => s.UserExists(validEmail)).Returns(false);
        var signUpDto = new SignUpDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = validPhone,
            Password = password
        };
        var result = userController.SignUp(signUpDto);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }
}
