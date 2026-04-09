namespace DarkKitchen.Domain.Exceptions;

public class EmailEmptyException()
    : ArgumentException("Email cannot be empty or whitespace.")
{
}
