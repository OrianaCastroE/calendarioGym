using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class PasswordTooShortExceptionTest
{
    private readonly string message = "Password must be at least 15 characters long.";

    [TestMethod]
    public void PasswordTooShortException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordTooShortException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
