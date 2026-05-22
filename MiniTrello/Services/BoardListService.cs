using Microsoft.EntityFrameworkCore;
using MiniTrello.Data;
using MiniTrello.DTOs;
using MiniTrello.Models;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Services;

public class BoardListService : IBoardListService
{
    private readonly MiniTrelloDbContext _db;

    public BoardListService(MiniTrelloDbContext db)
    {
        _db = db;
    }

    public async Task<List<BoardListDto>?> GetAllAsync(string userId, int boardId)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        var lists = await _db.BoardLists.Where(l => l.BoardId == boardId).Include(b => b.Cards).ToListAsync();

        var response = lists.Select(b => new BoardListDto
        {
            Id = b.Id,
            Name = b.Name,
            BoardId = b.BoardId,
            CreatedAt = b.CreatedAt,
            Cards = b.Cards.Select(l => new CardDto
            {
                Id = l.Id,
                Title = l.Title,
                BoardListId = l.BoardListId,
                Description = l.Description,
                CreatedAt = l.CreatedAt
            }).ToList()
        }).ToList();
        return response;
    }

    public async Task<BoardListDto?> GetByIdAsync(string userId, int boardId, int id)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        BoardList? boardlist = await _db.BoardLists.Include(l => l.Cards).FirstOrDefaultAsync(l => l.Id == id);
        if (boardlist == null || boardlist.BoardId != boardId)
            return null;

        BoardListDto response = new BoardListDto
        {
            Id = boardlist.Id,
            Name = boardlist.Name,
            BoardId = boardlist.BoardId,
            CreatedAt = boardlist.CreatedAt,
            Cards = boardlist.Cards.Select(l => new CardDto
            {
                Id = l.Id,
                Title = l.Title,
                BoardListId = l.BoardListId,
                Description = l.Description,
                CreatedAt = l.CreatedAt
            }).ToList()
        };
        return response;
    }

    public async Task<BoardListDto?> CreateAsync(string userId, int boardId, BoardListRequest boardlist)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        var newBoardList = new BoardList
        {
            Name = boardlist.Name,
            BoardId = boardId,
            CreatedAt = DateTime.UtcNow
        };
        _db.BoardLists.Add(newBoardList);
        await _db.SaveChangesAsync();

        var response = new BoardListDto
        {
            Id = newBoardList.Id,
            Name = newBoardList.Name,
            BoardId = newBoardList.BoardId,
            CreatedAt = newBoardList.CreatedAt,
            Cards = []
        };
        return response;
    }

    public async Task<bool> UpdateAsync(string userId, int boardId, int id, BoardListRequest update)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return false;

        BoardList? boardlist = await _db.BoardLists.FindAsync(id);
        if (boardlist == null || boardlist.BoardId != boardId)
            return false;

        boardlist.Name = update.Name;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, int boardId, int id)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return false;

        BoardList? boardlist = await _db.BoardLists.FindAsync(id);
        if (boardlist == null || boardlist.BoardId != boardId)
            return false;

        _db.BoardLists.Remove(boardlist);
        await _db.SaveChangesAsync();
        return true;
    }
}