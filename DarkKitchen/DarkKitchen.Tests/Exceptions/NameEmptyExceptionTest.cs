using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Exceptions;

[TestClass]
public class NameEmptyExceptionTest
{
    private readonly string message = "Name cannot be empty or whitespace.";

    [TestMethod]
    public void NameEmptyException_WithNoArgs_StoresMessage()
    {
        var ex = new NameEmptyException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
