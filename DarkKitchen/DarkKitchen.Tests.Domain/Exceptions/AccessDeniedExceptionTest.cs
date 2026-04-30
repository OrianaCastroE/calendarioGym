using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class AccessDeniedExceptionTest
{
    private readonly string message = "Test exception message.";

    [TestMethod]
    public void AccessDeniedException_WithMessage_StoresMessage()
    {
        var ex = new AccessDeniedException(message);

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
