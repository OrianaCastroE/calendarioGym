namespace DarkKitchen.Domain.Exceptions;

public class SurnameEmptyException()
    : ArgumentException("Surname cannot be empty or whitespace.")
{
}
