using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class UnauthorizedExceptionTest
{
    private readonly string message = "Test exception message.";

    [TestMethod]
    public void UnauthorizedException_WithMessage_StoresMessage()
    {
        var ex = new UnauthorizedException(message);

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
