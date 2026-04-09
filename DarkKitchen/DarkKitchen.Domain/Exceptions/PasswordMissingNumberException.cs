namespace DarkKitchen.Domain.Exceptions;

public class PasswordMissingNumberException()
    : ArgumentException("Password must contain at least one number.");
