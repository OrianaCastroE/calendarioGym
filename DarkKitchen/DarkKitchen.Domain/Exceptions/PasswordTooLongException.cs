namespace DarkKitchen.Domain.Exceptions;

public class PasswordTooLongException()
    : ArgumentException("Password cannot be longer than 25 characters.")
{
}
