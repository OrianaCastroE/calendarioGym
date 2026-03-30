using Domain.Entities;

namespace Domain.DTOs.UserDTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public Role Role { get; set; }
}
