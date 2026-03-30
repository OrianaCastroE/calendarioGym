using Domain.DTOs.UserDTO;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("signUp")]
    public IActionResult SignUp([FromBody] SignUpDto signUp)
    {
        if(!string.IsNullOrEmpty(signUp.Email))
        {
            if(!_userService.UserExists(signUp.Email))
            {
                return Created("User created", null);
            }
        }

        return BadRequest("User already exists");
    }
}
