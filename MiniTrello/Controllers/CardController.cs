using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrello.DTOs;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Controllers;

[ApiController]
[Route("/api/boards/{boardId}/lists/{listId}/cards")]
[Authorize]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int boardId, int listId)
    {
        var response = await _cardService.GetAllAsync(UserId, boardId, listId);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int boardId, int listId, int id)
    {
        var response = await _cardService.GetByIdAsync(UserId, boardId, listId, id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard(int boardId, int listId, CardRequest request)
    {
        var response = await _cardService.CreateAsync(UserId, boardId, listId, request);
        if (response == null)
            return NotFound();
        return CreatedAtAction(nameof(GetById), new { boardId, listId, id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCard(int boardId, int listId, int id, CardRequest request)
    {
        var updated = await _cardService.UpdateAsync(UserId, boardId, listId, id, request);
        if (!updated)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int boardId, int listId, int id)
    {
        var deleted = await _cardService.DeleteAsync(UserId, boardId, listId, id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
