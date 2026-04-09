using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class PasswordMissingNumberExceptionTest
{
    private readonly string message = "Password must contain at least one number.";

    [TestMethod]
    public void PasswordMissingNumberException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordMissingNumberException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
