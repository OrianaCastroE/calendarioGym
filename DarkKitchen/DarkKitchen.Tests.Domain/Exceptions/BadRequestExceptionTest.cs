using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class BadRequestExceptionTest
{
    private readonly string message = "Test exception message.";

    [TestMethod]
    public void BadRequestException_WithMessage_StoresMessage()
    {
        var ex = new BadRequestException(message);

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
