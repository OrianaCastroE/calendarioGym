namespace DarkKitchen.Models.UserDTOs;

public readonly record struct UserResponseDto(int id, string name, string surname, string email, string phone, string role);
