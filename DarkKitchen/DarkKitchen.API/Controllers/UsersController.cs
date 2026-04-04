using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.UserDTOs;
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

    [HttpPost("admin")]
    public IActionResult CreateUserWithRole([FromBody] CreateUserDto newUser)
    {
        _userService.CreateUserWithRole(newUser);
        return Created("User created correctly.", null);
    }

    [HttpPut]
    public IActionResult UpdateUser([FromBody] UserDto user)
    {
        _userService.UpdateUser(user);
        return Ok("User updated correctly.");
    }

    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? name, [FromQuery] string? surname)
    {
        var users = _userService.GetUsers(name!, surname!);
        return Ok(users);
    }

    [HttpDelete("{email}")]
    public IActionResult DeleteUser(string email)
    {
        _userService.DeleteUser(email);
        return Ok("User deleted correctly.");
    }
}
