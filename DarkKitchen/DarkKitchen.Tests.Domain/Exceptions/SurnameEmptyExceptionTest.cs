using DarkKitchen.Domain.Exceptions;

namespace DarkKitchen.Tests.Domain.Exceptions;

[TestClass]
public class SurnameEmptyExceptionTest
{
    private readonly string message = "Surname cannot be empty or whitespace.";

    [TestMethod]
    public void SurnameEmptyException_WithNoArgs_StoresMessage()
    {
        var ex = new SurnameEmptyException();

        Assert.IsNotNull(ex);
        Assert.AreEqual(message, ex.Message);
    }
}
