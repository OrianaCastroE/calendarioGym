using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
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
    private User? _user;

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

        _user = new User
        {
            Name = _validUser.Name!,
            Surname = _validUser.Surname!,
            Email = _validUser.Email!,
            Phone = _validUser.Phone!,
            Password = _validUser.Password!,
            Role = Role.Client,
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
    public void CreateUser_WhenInvalidName_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Name = string.Empty;

        NameEmptyException ex = Assert.ThrowsException<NameEmptyException>(() =>
            _userService!.CreateUser(_validUser));

        Assert.AreEqual("Name cannot be empty or whitespace.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenInvalidSurname_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Surname = null;
        SurnameEmptyException ex = Assert.ThrowsException<SurnameEmptyException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Surname cannot be empty or whitespace.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenInvalidEmail_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Email = "invalidEmail";
        InvalidEmailException ex = Assert.ThrowsException<InvalidEmailException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Email is not valid.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenLongPassword_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "longPassworddddddddddddddddddd";
        PasswordTooLongException ex = Assert.ThrowsException<PasswordTooLongException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password cannot be longer than 25 characters.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenShortPassword_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "shortPassword";
        PasswordTooShortException ex = Assert.ThrowsException<PasswordTooShortException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password must be at least 15 characters long.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_PasswordDoesNotContainsUpperLetter_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "lowerpasswordddd";
        PasswordMissingUppercaseException ex = Assert.ThrowsException<PasswordMissingUppercaseException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password must contain at least one uppercase letter.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_PasswordDoesNotContainsLowerLetter_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "UPPERPASSWORDDDD";
        PasswordMissingLowercaseException ex = Assert.ThrowsException<PasswordMissingLowercaseException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password must contain at least one lowercase letter.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenPasswordDoesNotContainSpecialCharacter_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "PassWithoutSpecialChar123";
        PasswordMissingSpecialCharacterException ex = Assert.ThrowsException<PasswordMissingSpecialCharacterException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password must contain at least one special character.", ex.Message);
    }

    [TestMethod]
    public void CreateUser_WhenPasswordDoesNotContainANumber_ShouldThrowExceptionAndNotCreateUser()
    {
        _validUser!.Password = "PasswordWithoutNumber!";
        PasswordMissingNumberException ex = Assert.ThrowsException<PasswordMissingNumberException>(() =>
            _userService!.CreateUser(_validUser));
        Assert.AreEqual("Password must contain at least one number.", ex.Message);
    }

    [TestMethod]
    public void UpdateUser_WhenUserNotFound_ShouldThrowExceptionAndNotUpdateUser()
    {
        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail(It.IsAny<string>()))
            .Returns((User)null);
        UserNotFoundException ex = Assert.ThrowsException<UserNotFoundException>(() =>
            _userService!.UpdateUser(new UserDto { Email = "fakeUser@gmail.com" }));
        Assert.AreEqual("User not found.", ex.Message);
    }

    [TestMethod]
    public void UpdateUser_WhenEmailIsNull_ShouldThrowExceptionAndNotUpdateUser()
    {
        EmailEmptyException ex = Assert.ThrowsException<EmailEmptyException>(() =>
            _userService!.UpdateUser(new UserDto { Email = null }));
        Assert.AreEqual("Email cannot be empty or whitespace.", ex.Message);
    }

    [TestMethod]
    public void UpdateUser_WhenValidUser_ShouldUpdateUser()
    {
        var dto = new UserDto
        {
            Name = "UpdatedName",
            Surname = "UpdatedSurname",
            Email = _user.Email,
            Phone = "099999999",
            Password = "UpdatedPassword1!"
        };

        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail(_user.Email))
            .Returns(_user);

        _userRepositoryMock!
            .Setup(repository => repository.Update(It.IsAny<User>()))
            .Returns((User user) => user);

        _userService.UpdateUser(dto);

        User result = _userRepositoryMock.Object.GetByEmail(_user.Email)!;
        Assert.AreEqual("UpdatedName", result.Name);
        Assert.AreEqual("UpdatedSurname", result.Surname);
    }

    [TestMethod]
    public void GetUsers_WhenThereAreNoUsers_ShouldReturnEmptyList()
    {
        _userRepositoryMock!
            .Setup(repository => repository.GetUsers("validName", "validSurname"))
            .Returns([]);

        List<UserResponseDto> result = _userService!.GetUsers("validName", "validSurname");

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetUsers_WhenRepositoryReturnsUsers_ShouldReturnMappedDtos()
    {
        _user = new User
        {
            Name = "validName",
            Surname = "validSurname",
            Email = "validEmail@gmail.com",
            Phone = "099123456",
            Password = "PasswordSegura123!",
            Role = Role.Client,
        };

        var users = new List<User> { _user };

        _userRepositoryMock!
            .Setup(repository => repository.GetUsers("validName", "validSurname"))
            .Returns(users);

        List<UserResponseDto> result = _userService!.GetUsers("validName", "validSurname");

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("validName", result[0].Name);
        Assert.AreEqual("validSurname", result[0].Surname);
        Assert.AreEqual("validEmail@gmail.com", result[0].Email);
    }

    [TestMethod]
    public void GetUsers_WhenUsersExist_ShouldReturnUsers()
    {
        _user = new User
        {
            Name = "validName",
            Surname = "validSurname",
            Email = "validEmail@gmail.com",
            Phone = "099123456",
            Password = "PasswordSegura123!",
            Role = Role.Client,
        };

        var users = new List<User> { _user };
        _userRepositoryMock!
            .Setup(repository => repository.GetUsers("validName", "validSurname"))
            .Returns(users);

        var result = _userService!.GetUsers("validName", "validSurname");

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("validName", result[0].Name);
    }

    [TestMethod]
    public void DeleteUser_WhenUserExists_ShouldDeleteUser()
    {
        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail("validEmail@gmail.com"))
            .Returns(_user!);

        _userService!.DeleteUser("validEmail@gmail.com");

        _userRepositoryMock!
            .Verify(repository => repository.Delete(_user!), Times.Once);

        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail("validEmail@gmail.com"))
            .Returns((User?)null);
    }

    [TestMethod]
    public void DeleteUser_WhenUserDoesNotExist_ShouldThrowException()
    {
        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail("fakeUser@gmail.com"))
            .Returns((User?)null);

        UserNotFoundException ex = Assert.ThrowsException<UserNotFoundException>(() =>
            _userService!.DeleteUser("fakeUser@gmail.com"));

        Assert.AreEqual("User not found.", ex.Message);
    }

    [TestMethod]
    public void DeleteUser_WhenEmailIsEmpty_ShouldThrowException()
    {
        _userRepositoryMock!
            .Setup(repository => repository.GetByEmail(It.IsAny<string>()))
            .Returns((User?)null);

        EmailEmptyException ex = Assert.ThrowsException<EmailEmptyException>(() =>
            _userService!.DeleteUser(string.Empty));

        Assert.AreEqual("Email cannot be empty or whitespace.", ex.Message);

        _userRepositoryMock!
            .Verify(repository => repository.GetByEmail(It.IsAny<string>()), Times.Never);

        _userRepositoryMock!
            .Verify(repository => repository.Delete(It.IsAny<User>()), Times.Never);
    }
}
