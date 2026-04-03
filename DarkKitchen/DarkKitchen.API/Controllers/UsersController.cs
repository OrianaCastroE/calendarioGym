using Domain.DTOs.UserDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public IActionResult SignUp([FromBody] UserDto newUser)
    {
        try
        {
            _userService.CreateUser(newUser);
            return Created("User created correctly.", null);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("admin")]
    public IActionResult CreateUserWithRole([FromBody] CreateUserDto newUser)
    {
        try
        {
            _userService.CreateUserWithRole(newUser);
            return Created("User created correctly.", null);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateUser([FromBody] UserDto user)
    {
        try
        {
            _userService.UpdateUser(user);
            return Ok("User updated correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? name, [FromQuery] string? surname)
    {
        try
        {
            var users = _userService.GetUsers(name!, surname!);
            return Ok(users);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{email}")]
    public IActionResult DeleteUser(string email)
    {
        try
        {
            _userService.DeleteUser(email);
            return Ok("User deleted correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
