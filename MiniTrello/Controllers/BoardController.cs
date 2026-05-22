
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrello.DTOs;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Controllers;

[ApiController]
[Route("api/boards")]
[Authorize]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _boardService.GetAllAsync(UserId);
        return Ok(boards);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _boardService.GetByIdAsync(UserId, id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoard(BoardRequest request)
    {
        var response = await _boardService.CreateAsync(UserId, request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard(int id, BoardRequest request)
    {
        bool response = await _boardService.UpdateAsync(UserId, id, request);
        if (!response)
            return NotFound();
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        bool response = await _boardService.DeleteAsync(UserId, id);
        if (!response)
            return NotFound();
        return NoContent();


    }
}