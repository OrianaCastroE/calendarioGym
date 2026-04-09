namespace DarkKitchen.Domain.Exceptions;

public class PasswordTooShortException()
    : ArgumentException("Password must be at least 15 characters long.")
{
}
