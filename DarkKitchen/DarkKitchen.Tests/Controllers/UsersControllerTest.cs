using DarkKitchen.API.Controllers;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DarkKitchen.Tests.Controllers;

[TestClass]
public class UsersControllerTest
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
            new UserResponseDto { Id = 1, Name = "Name", Surname = "Surname", Email = validEmail, Phone = validPhone, Role = Role.Client.ToString() }
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
            Role = Role.Admin.ToString()
        };
    }

    [TestMethod]
    public void UserSignUp_ValidUserWithoutRole_UserRegisteredAsClient()
    {
        userServiceMock.Setup(s => s.CreateUser(validUser!));
        var result = userController.SignUp(validUser!);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void UserSignUp_UserAlreadyExists_ThrowsBadRequestException()
    {
        userServiceMock.Setup(s => s.CreateUser(validUser!)).Throws(new BadRequestException("User already exists."));
        var result = userController.SignUp(validUser!);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void UserSignUp_InvalidEmailFormat_ThrowsBadRequestException()
    {
        var invalidEmailUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = "invalid-email",
            Phone = validPhone,
            Password = password
        };
        userServiceMock.Setup(s => s.CreateUser(invalidEmailUser)).Throws(new BadRequestException("Invalid email address."));
        var result = userController.SignUp(invalidEmailUser);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void UserSignUp_InvalidPhoneFormat_ThrowsBadRequestException()
    {
        var invalidPhoneUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = "invalid-phone",
            Password = password
        };
        userServiceMock.Setup(s => s.CreateUser(invalidPhoneUser)).Throws(new BadRequestException("Invalid phone number."));
        var result = userController.SignUp(invalidPhoneUser);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void UserSignUp_InvalidPassword_ThrowsBadRequestException()
    {
        var invalidPasswordUser = new UserDto()
        {
            Name = "Name",
            Surname = "Surname",
            Email = validEmail,
            Phone = validPhone,
            Password = "short"
        };
        userServiceMock.Setup(s => s.CreateUser(invalidPasswordUser)).Throws(new BadRequestException("Password does not meet complexity requirements."));
        var restult = userController.SignUp(invalidPasswordUser);
    }

    [TestMethod]
    public void CreateUserWithRole_ValidData_ReturnsCreated()
    {
        userServiceMock.Setup(s => s.CreateUserWithRole(validAdminUser!));
        var result = userController.CreateUserWithRole(validAdminUser!);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(201, resultObj.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void CreateUserWithRole_UserAlreadyExists_ThrowsBadRequestException()
    {
        userServiceMock.Setup(s => s.CreateUserWithRole(validAdminUser!)).Throws(new BadRequestException("User already exists."));
        var result = userController.CreateUserWithRole(validAdminUser!);
    }

    [TestMethod]
    public void UpdateUser_ExistingUser_ReturnsOk()
    {
        userServiceMock.Setup(s => s.UpdateUser(validUser!));
        var result = userController.UpdateUser(validUser!);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void UpdateUser_UserNotFound_ThrowsNotFoundException()
    {
        userServiceMock.Setup(s => s.UpdateUser(validUser!)).Throws(new NotFoundException("User not found."));
        var result = userController.UpdateUser(validUser!);
    }

    [TestMethod]
    public void GetUsers_ValidFilter_ReturnsOk()
    {
        userServiceMock.Setup(s => s.GetUsers("Name", "Surname")).Returns(users!);
        var result = userController.GetUsers("Name", "Surname");
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void GetUsers_NoUsersFound_ThrowsNotFoundException()
    {
        userServiceMock.Setup(s => s.GetUsers("Name", "Surname")).Throws(new NotFoundException("No users found."));
        var result = userController.GetUsers("Name", "Surname");
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_ReturnsOk()
    {
        userServiceMock.Setup(s => s.DeleteUser(validEmail));
        var result = userController.DeleteUser(validEmail);
        var resultObj = result as ObjectResult;

        Assert.IsNotNull(resultObj);
        Assert.AreEqual(200, resultObj.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public void DeleteUser_UserNotFound_ThrowsNotFoundException()
    {
        userServiceMock.Setup(s => s.DeleteUser(validEmail)).Throws(new NotFoundException("User not found."));
        var result = userController.DeleteUser(validEmail);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public void DeleteUser_EmptyEmail_ThrowsBadRequestException()
    {
        userServiceMock.Setup(s => s.DeleteUser(string.Empty)).Throws(new BadRequestException("Email can't be empty."));
        var result = userController.DeleteUser(string.Empty);
    }
}
