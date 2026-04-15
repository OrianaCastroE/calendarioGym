namespace DarkKitchen.Models.UserDTOs;

public readonly record struct CreateUserDto(string? name, string? surname, string? email, string? phone, string? password, string? role);
