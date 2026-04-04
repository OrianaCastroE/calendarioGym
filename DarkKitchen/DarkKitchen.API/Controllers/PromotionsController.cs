using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.PromotionDTOs;
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

    [HttpPut]
    public IActionResult UpdatePromotion([FromBody] PromotionDto updatedPromotion)
    {
        try
        {
            _promotionService.UpdatePromotion(updatedPromotion);
            return Ok("Promotion updated correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{promotionId}/products/{productCode}")]
    public IActionResult AssociateProduct(int promotionId, string productCode)
    {
        try
        {
            _promotionService.AssociateProduct(promotionId, productCode);
            return Ok("Product associated correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{promotionId}/products/{productCode}")]
    public IActionResult DisassociateProduct(int promotionId, string productCode)
    {
        try
        {
            _promotionService.DisassociateProduct(promotionId, productCode);
            return Ok("Product disassociated correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetPromotions([FromQuery] DateTime? date, [FromQuery] string? productLine, [FromQuery] string? productName)
    {
        try
        {
            var promotions = _promotionService.GetPromotions(date, productLine, productName);
            return Ok(promotions);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
