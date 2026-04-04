using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.PromotionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/promotions")]
public class PromotionsController(IPromotionService promotionService) : ControllerBase
{
    private readonly IPromotionService _promotionService = promotionService;

    [HttpPost]
    public IActionResult CreatePromotion([FromBody] PromotionDto newPromotion)
    {
        _promotionService.CreatePromotion(newPromotion);
        return Created(string.Empty, newPromotion);
    }

    [HttpPut]
    public IActionResult UpdatePromotion([FromBody] PromotionDto updatedPromotion)
    {
        _promotionService.UpdatePromotion(updatedPromotion);
        return Ok("Promotion updated correctly.");
    }

    [HttpPost("{promotionId}/products/{productCode}")]
    public IActionResult AssociateProduct(int promotionId, string productCode)
    {
        _promotionService.AssociateProduct(promotionId, productCode);
        return Ok("Product associated correctly.");
    }

    [HttpDelete("{promotionId}/products/{productCode}")]
    public IActionResult DisassociateProduct(int promotionId, string productCode)
    {
        _promotionService.DisassociateProduct(promotionId, productCode);
        return Ok("Product disassociated correctly.");
    }

    [HttpGet]
    public IActionResult GetPromotions([FromQuery] DateTime? date, [FromQuery] string? productLine, [FromQuery] string? productName)
    {
        var promotions = _promotionService.GetPromotions(date, productLine, productName);
        return Ok(promotions);
    }
}
