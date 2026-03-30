using DarkKitchen.API.Controllers;
using Domain.DTOs.UserDTOs;
using Domain.Entities;
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
    private UserDto? validUser;
    private UserDto? updatedUser;
    private CreateUserDto? validAdminUser;
    private List<UserResponseDto>? users;
    private Mock<IUserService>? userServiceMock;
    private UsersController? userController;

    [TestInitialize]
    public void Setup()
    {
        userServiceMock = new Mock<IUserService>();
        userController = new UsersController(userServiceMock.Object);
        users =
        [
            new UserResponseDto { Id = 1, Name = "Name", Surname = "Surname", Email = validEmail, Phone = validPhone, Role = Role.Client }
        ];
        validUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = validPhone,
            Password = password
        };
        updatedUser = new UserDto()
        {
            Name = "UpdatedName",
            Surname = "UpdatedSurname",
            Email = validEmail,
            Phone = validPhone,
            Password = password
        };
        validAdminUser = new CreateUserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = validPhone,
            Password = password,
            Role = Role.Admin
        };
    }

    [TestMethod]
    public void UserSignUp_ValidUserWithoutRole_UserRegisteredAsClient()
    {
        userServiceMock.Setup(s => s.CreateUser(validUser!));
        var result = userController.SignUp(validUser!);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    public void UserSignUp_UserAlreadyExists_ReturnsBadRequest()
    {
        userServiceMock.Setup(s => s.CreateUser(validUser!)).Throws(new Exception("User already exists."));
        var result = userController.SignUp(validUser!);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void UserSignUp_InvalidEmailFormat_ReturnsBadRequest()
    {
        var invalidEmailUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = "invalid-email",
            Phone = validPhone,
            Password = password
        };
        userServiceMock.Setup(s => s.CreateUser(invalidEmailUser)).Throws(new Exception("Invalid email address."));
        var result = userController.SignUp(invalidEmailUser);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void UserSignUp_InvalidPhoneFormat_ReturnsBadRequest()
    {
        var invalidPhoneUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = "invalid-phone",
            Password = password
        };
        userServiceMock.Setup(s => s.CreateUser(invalidPhoneUser)).Throws(new Exception("Invalid phone number."));
        var result = userController.SignUp(invalidPhoneUser);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void UserSignUp_InvalidPassword_ReturnsBadRequest()
    {
        var invalidPasswordUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = validPhone,
            Password = "short"
        };
        userServiceMock.Setup(s => s.CreateUser(invalidPasswordUser)).Throws(new Exception("Password does not meet complexity requirements."));
        var restult = userController.SignUp(invalidPasswordUser);
        var resultObj = restult as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void CreateUserWithRole_ValidData_ReturnsCreated()
    {
        userServiceMock.Setup(s => s.CreateUserWithRole(validAdminUser!));
        var result = userController.CreateUserWithRole(validAdminUser!);
        var resultObj = result as CreatedResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    public void CreateUserWithRole_UserAlreadyExists_ReturnsBadRequest()
    {
        userServiceMock.Setup(s => s.CreateUserWithRole(validAdminUser!)).Throws(new Exception("User already exists."));
        var result = userController.CreateUserWithRole(validAdminUser!);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }

    [TestMethod]
    public void UpdateUser_ExistingUser_ReturnsOk()
    {
        userServiceMock.Setup(s => s.UpdateUser(validUser!));
        var result = userController.UpdateUser(validUser!);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void UpdateUser_UserNotFound_ReturnsNotFound()
    {
        userServiceMock.Setup(s => s.UpdateUser(validUser!)).Throws(new Exception("User not found."));
        var result = userController.UpdateUser(validUser!);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetUsers_ValidFilter_ReturnsOk()
    {
        userServiceMock.Setup(s => s.GetUsers("Name", "Surname")).Returns(users!);
        var result = userController.GetUsers("Name", "Surname");
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void GetUsers_NoUsersFound_ReturnsNotFound()
    {
        userServiceMock.Setup(s => s.GetUsers("Name", "Surname")).Throws(new Exception("No users found."));
        var result = userController.GetUsers("Name", "Surname");
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_ReturnsOk()
    {
        userServiceMock.Setup(s => s.DeleteUser(validEmail));
        var result = userController.DeleteUser(validEmail);
        var resultObj = result as OkObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    public void DeleteUser_UserNotFound_ReturnsNotFound()
    {
        userServiceMock.Setup(s => s.DeleteUser(validEmail)).Throws(new Exception("User not found."));
        var result = userController.DeleteUser(validEmail);
        var resultObj = result as NotFoundObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(404, resultObj.StatusCode);
    }

    [TestMethod]
    public void DeleteUser_EmptyEmail_ReturnsBadRequest()
    {
        var result = userController.DeleteUser(string.Empty);
        var resultObj = result as BadRequestObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(400, resultObj.StatusCode);
    }
}
