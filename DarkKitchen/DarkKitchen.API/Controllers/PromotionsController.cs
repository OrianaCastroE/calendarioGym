using Domain.DTOs.PromotionDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController(IPromotionService promotionService) : ControllerBase
{
    private readonly IPromotionService _promotionService = promotionService;

    [HttpPost]
    public IActionResult CreatePromotion([FromBody] PromotionDto newPromotion)
    {
        try
        {
            _promotionService.CreatePromotion(newPromotion);
            return Created(string.Empty, newPromotion);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
