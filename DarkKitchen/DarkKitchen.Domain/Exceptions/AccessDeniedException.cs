namespace DarkKitchen.Domain.Exceptions;

public class AccessDeniedException(string message) : Exception(message)
{
}
