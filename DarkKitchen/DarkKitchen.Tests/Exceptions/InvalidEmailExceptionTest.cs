using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class InvalidEmailExceptionTest
{
    private readonly string message = "Email is not valid.";

    [TestMethod]
    public void InvalidEmailException_WithNoArgs_StoresMessage()
    {
        var ex = new InvalidEmailException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
