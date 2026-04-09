namespace DarkKitchen.Domain.Exceptions;

public class PasswordMissingUppercaseException()
    : ArgumentException("Password must contain at least one uppercase letter.")
{
}
