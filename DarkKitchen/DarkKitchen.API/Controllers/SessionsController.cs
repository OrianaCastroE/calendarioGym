using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.SessionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/sessions")]
[ApiController]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpPost]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var response = _sessionService.Login(loginDto);
        return Ok(response);
    }
}
