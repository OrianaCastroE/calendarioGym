using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.SessionDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DarkKitchen.Services;

public class SessionService(IUserRepository userRepository, IConfiguration configuration) : ISessionService
{
    public LoginResponseDto Login(LoginDto loginDto)
    {
        var user = userRepository.GetByEmail(loginDto.email!);
        if(user == null || !BCrypt.Net.BCrypt.Verify(loginDto.password, user.Password))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        return new LoginResponseDto(GenerateToken(user));
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
