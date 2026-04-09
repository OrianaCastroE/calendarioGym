using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class PasswordMissingSpecialCharacterExceptionTest
{
    private readonly string message = "Password must contain at least one special character.";

    [TestMethod]
    public void PasswordMissingSpecialCharacterException_WithNoArgs_StoresMessage()
    {
        var ex = new PasswordMissingSpecialCharacterException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
