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

    [Authorize(Policy = "CreateUser")]
    [HttpPost("admin")]
    public IActionResult CreateUserWithRole([FromBody] CreateUserDto newUser)
    {
        _userService.CreateUserWithRole(newUser);
        return Created("User created correctly.", null);
    }

    [Authorize(Policy = "UpdateUser")]
    [HttpPut]
    public IActionResult UpdateUser([FromBody] UserDto user)
    {
        _userService.UpdateUser(user);
        return Ok("User updated correctly.");
    }

    [Authorize(Policy = "GetUsers")]
    [HttpGet]
    public IActionResult GetUsers([FromQuery] UserFiltersDto filter)
    {
        var users = _userService.GetUsers(filter);
        return Ok(users);
    }

    [Authorize(Policy = "DeleteUser")]
    [HttpDelete("{email}")]
    public IActionResult DeleteUser(string email)
    {
        _userService.DeleteUser(email);
        return Ok("User deleted correctly.");
    }
}
