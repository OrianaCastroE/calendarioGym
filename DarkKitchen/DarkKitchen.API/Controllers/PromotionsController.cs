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

    [HttpPut("{id}")]
    public IActionResult UpdatePromotion(int id, [FromBody] PromotionDto updatedPromotion)
    {
        _promotionService.UpdatePromotion(id, updatedPromotion);
        return Ok("Promotion updated correctly.");
    }

    [HttpPut("{id}/products")]
    public IActionResult UpdatePromotionProducts(int id, [FromBody] UpdatePromotionProductsDto dto)
    {
        _promotionService.UpdatePromotionProducts(id, dto.Products);
        return Ok("Promotion products updated correctly.");
    }

    [HttpGet]
    public IActionResult GetPromotions([FromQuery] DateTime? date, [FromQuery] string? productLine, [FromQuery] string? productName)
    {
        var promotions = _promotionService.GetPromotions(date, productLine, productName);
        return Ok(promotions);
    }
}
