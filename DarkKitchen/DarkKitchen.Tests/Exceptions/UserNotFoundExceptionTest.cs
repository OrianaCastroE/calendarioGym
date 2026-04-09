using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class UserNotFoundExceptionTest
{
    private readonly string message = "User not found.";

    [TestMethod]
    public void UserNotFoundException_WithNoArgs_StoresMessage()
    {
        var ex = new UserNotFoundException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
