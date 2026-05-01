using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public IActionResult SignUp([FromBody] UserDto newUser)
    {
        _userService.CreateUser(newUser);
        return Created("User created correctly.", null);
    }

    [Authorize(Policy = nameof(Permission.CreateUser))]
    [HttpPost("admin")]
    public IActionResult CreateUserWithRole([FromBody] CreateUserDto newUser)
    {
        _userService.CreateUserWithRole(newUser);
        return Created("User created correctly.", null);
    }

    [Authorize(Policy = nameof(Permission.UpdateUser))]
    [HttpPut]
    public IActionResult UpdateUser([FromBody] UserDto user)
    {
        _userService.UpdateUser(user);
        return Ok("User updated correctly.");
    }

    [Authorize(Policy = nameof(Permission.GetUsers))]
    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? name, [FromQuery] string? surname)
    {
        var users = _userService.GetUsers(new UserFiltersDto(name, surname));
        return Ok(users);
    }

    [Authorize(Policy = nameof(Permission.DeleteUser))]
    [HttpDelete("{email}")]
    public IActionResult DeleteUser(string email)
    {
        _userService.DeleteUser(email);
        return Ok("User deleted correctly.");
    }
}
