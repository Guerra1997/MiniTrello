using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrello.DTOs;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Controllers;

[ApiController]
[Route("/api/boards/{boardId}/lists")]
[Authorize]
public class BoardListController : ControllerBase
{
    private readonly IBoardListService _boardListService;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public BoardListController(IBoardListService boardListService)
    {
        _boardListService = boardListService;

    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int boardId)
    {
        var response = await _boardListService.GetAllAsync(UserId, boardId);
        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int boardId, int id)
    {
        var response = await _boardListService.GetByIdAsync(UserId, boardId, id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBoardList(int boardId, BoardListRequest request)
    {
        var response = await _boardListService.CreateAsync(UserId, boardId, request);
        if (response == null)
            return NotFound();

        return CreatedAtAction(nameof(GetById), new { boardId = response.BoardId, id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoardList(int boardId, int id, BoardListRequest request)
    {
        var response = await _boardListService.UpdateAsync(UserId, boardId, id, request);
        if (!response)
            return NotFound();
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoardList(int boardId, int id)
    {
        var response = await _boardListService.DeleteAsync(UserId, boardId, id);
        if (!response)
            return NotFound();
        return NoContent();
    }
}