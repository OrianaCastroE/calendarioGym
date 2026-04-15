namespace DarkKitchen.Models.UserDTOs;

public readonly record struct UserDto(string? name, string? surname, string? email, string? phone, string? password);
