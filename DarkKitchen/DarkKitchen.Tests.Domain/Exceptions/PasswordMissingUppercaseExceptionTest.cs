using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class PasswordMissingUppercaseExceptionTest
{
    private readonly string message = "Password must contain at least one uppercase letter.";

    [TestMethod]
    public void PasswordMissingUppercaseException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordMissingUppercaseException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
