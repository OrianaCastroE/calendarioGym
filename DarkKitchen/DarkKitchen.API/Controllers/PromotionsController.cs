using DarkKitchen.Domain.Interfaces.Services;
using DarkKitchen.Models.PromotionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/promotions")]
public class PromotionsController(IPromotionService promotionService) : ControllerBase
{
    private readonly IPromotionService _promotionService = promotionService;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreatePromotion([FromBody] PromotionDto newPromotion)
    {
        _promotionService.CreatePromotion(newPromotion);
        return Created(string.Empty, newPromotion);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult UpdatePromotion(int id, [FromBody] PromotionDto updatedPromotion)
    {
        _promotionService.UpdatePromotion(id, updatedPromotion);
        return Ok("Promotion updated correctly.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/products")]
    public IActionResult UpdatePromotionProducts(int id, [FromBody] UpdatePromotionProductsDto dto)
    {
        _promotionService.UpdatePromotionProducts(id, dto.products);
        return Ok("Promotion products updated correctly.");
    }

    [Authorize(Roles = "Admin, Client")]
    [HttpGet]
    public IActionResult GetPromotions([FromQuery] PromotionFiltersDto filter)
    {
        var promotions = _promotionService.GetPromotions(filter);
        return Ok(promotions);
    }
}
