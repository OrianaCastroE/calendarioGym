using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.SessionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpPost]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = _sessionService.Login(loginDto);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
