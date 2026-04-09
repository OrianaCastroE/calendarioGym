using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class PasswordMissingLowercaseExceptionTest
{
    private readonly string message = "Password must contain at least one lowercase letter.";

    [TestMethod]
    public void PasswordMissingLowercaseException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordMissingLowercaseException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
