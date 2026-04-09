using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Tests.Domain.Entities;

[TestClass]
public class UserTest
{
    private readonly int id = 1;
    private readonly string name = "Name";
    private readonly string surname = "Surname";
    private readonly string email = "valid@email.com";
    private readonly string phone = "+59899123123";
    private readonly string password = "ValidPassword1!";
    private readonly Role role = Role.Client;
    private User? user;

    [TestInitialize]
    public void Setup()
    {
        user = new User
        {
            Id = id,
            Name = name,
            Surname = surname,
            Email = email,
            Phone = phone,
            Password = password,
            Role = role
        };
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectName()
    {
        Assert.AreEqual(name, user!.Name);
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectSurname()
    {
        Assert.AreEqual(surname, user!.Surname);
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectEmail()
    {
        Assert.AreEqual(email, user!.Email);
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectPhone()
    {
        Assert.AreEqual(phone, user!.Phone);
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectPassword()
    {
        Assert.AreEqual(password, user!.Password);
    }

    [TestMethod]
    public void User_WithValidData_HasCorrectRole()
    {
        Assert.AreEqual(role, user!.Role);
    }
}
