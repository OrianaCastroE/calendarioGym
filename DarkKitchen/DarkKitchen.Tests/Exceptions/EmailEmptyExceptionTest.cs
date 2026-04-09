using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class EmailEmptyExceptionTest
{
    private readonly string message = "Email cannot be empty or whitespace.";

    [TestMethod]
    public void EmailEmptyException_WithNoArgs_StoresMessage()
    {
        var ex = new EmailEmptyException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
