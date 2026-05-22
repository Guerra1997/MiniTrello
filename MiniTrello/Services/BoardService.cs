using Microsoft.EntityFrameworkCore;
using MiniTrello.Data;
using MiniTrello.DTOs;
using MiniTrello.Models;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Services;

public class BoardService : IBoardService
{
    private readonly MiniTrelloDbContext _db;

    public BoardService(MiniTrelloDbContext db)
    {
        _db = db;
    }

    public async Task<List<BoardDto>> GetAllAsync(string userId)
    {
        var boards = await _db.Boards.Where(b => b.UserId == userId).Include(b => b.Lists).ToListAsync();

        var response = boards.Select(b => new BoardDto
        {
            Id = b.Id,
            UserId = userId,
            Name = b.Name,
            Description = b.Description,
            CreatedAt = b.CreatedAt,
            Lists = b.Lists.Select(l => new BoardListDto
            {
                Id = l.Id,
                Name = l.Name,
                BoardId = l.BoardId,
                CreatedAt = l.CreatedAt
            }).ToList()
        }).ToList();

        return response;
    }


    public async Task<BoardDto?> GetByIdAsync(string userId, int id)
    {
        var board = await _db.Boards.Include(b => b.Lists).FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        if (board == null)
            return null;

        BoardDto response = new BoardDto
        {
            Id = board.Id,
            UserId = userId,
            Name = board.Name,
            Description = board.Description,
            CreatedAt = board.CreatedAt,
            Lists = board.Lists.Select(l => new BoardListDto
            {
                Id = l.Id,
                Name = l.Name,
                BoardId = l.BoardId,
                CreatedAt = l.CreatedAt
            }).ToList()
        };
        return response;
    }

    public async Task<BoardDto> CreateAsync(string userId, BoardRequest request)
    {
        var newBoard = new Board
        {
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Boards.Add(newBoard);
        await _db.SaveChangesAsync();

        var response = new BoardDto
        {
            Id = newBoard.Id,
            UserId = userId,
            Name = newBoard.Name,
            Description = newBoard.Description,
            CreatedAt = newBoard.CreatedAt,
            Lists = []
        };
        return response;
    }

    public async Task<bool> UpdateAsync(string userId, int id, BoardRequest request)
    {
        var board = await _db.Boards.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        if (board == null)
            return false;

        board.Name = request.Name;
        board.Description = request.Description;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, int id)
    {
        var board = await _db.Boards.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        if (board == null)
            return false;

        _db.Boards.Remove(board);
        await _db.SaveChangesAsync();
        return true;
    }
}