namespace DarkKitchen.Domain.Exceptions;

public class PasswordMissingLowercaseException()
    : ArgumentException("Password must contain at least one lowercase letter.")
{
}
