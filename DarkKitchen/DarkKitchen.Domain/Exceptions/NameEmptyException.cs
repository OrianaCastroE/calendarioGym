namespace DarkKitchen.Domain.Exceptions;

public class NameEmptyException()
    : ArgumentException("Name cannot be empty or whitespace.");
