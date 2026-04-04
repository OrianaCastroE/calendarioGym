using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class NotFoundExceptionTest
{
    private readonly string message = "Test exception message.";

    [TestMethod]
    public void NotFoundException_WithMessage_StoresMessage()
    {
        var ex = new NotFoundException(message);

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
