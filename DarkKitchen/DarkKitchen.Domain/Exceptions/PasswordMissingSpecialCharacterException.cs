namespace DarkKitchen.Domain.Exceptions;

public class PasswordMissingSpecialCharacterException()
    : ArgumentException("Password must contain at least one special character.");
