using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class PasswordTooLongExceptionTest
{
    private readonly string message = "Password cannot be longer than 25 characters.";

    [TestMethod]
    public void PasswordTooLongException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordTooLongException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
