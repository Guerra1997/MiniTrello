using Microsoft.EntityFrameworkCore;
using MiniTrello.Data;
using MiniTrello.DTOs;
using MiniTrello.Models;
using MiniTrello.Services.Interfaces;

namespace MiniTrello.Services;

public class CardService : ICardService
{
    private readonly MiniTrelloDbContext _db;

    public CardService(MiniTrelloDbContext db)
    {
        _db = db;
    }

    public async Task<List<CardDto>?> GetAllAsync(string userId, int boardId, int listId)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        BoardList? list = await _db.BoardLists.FindAsync(listId);
        if (list == null || list.BoardId != boardId)
            return null;

        var cards = await _db.Cards.Where(c => c.BoardListId == listId).ToListAsync();

        return cards.Select(c => new CardDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            BoardListId = c.BoardListId,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    public async Task<CardDto?> GetByIdAsync(string userId, int boardId, int listId, int id)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        BoardList? list = await _db.BoardLists.FindAsync(listId);
        if (list == null || list.BoardId != boardId)
            return null;

        Card? card = await _db.Cards.FindAsync(id);
        if (card == null || card.BoardListId != listId)
            return null;

        return new CardDto
        {
            Id = card.Id,
            Title = card.Title,
            Description = card.Description,
            BoardListId = card.BoardListId,
            CreatedAt = card.CreatedAt
        };
    }

    public async Task<CardDto?> CreateAsync(string userId, int boardId, int listId, CardRequest request)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return null;

        BoardList? list = await _db.BoardLists.FindAsync(listId);
        if (list == null || list.BoardId != boardId)
            return null;

        var card = new Card
        {
            Title = request.Title,
            Description = request.Description,
            BoardListId = listId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Cards.Add(card);
        await _db.SaveChangesAsync();

        return new CardDto
        {
            Id = card.Id,
            Title = card.Title,
            Description = card.Description,
            BoardListId = card.BoardListId,
            CreatedAt = card.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(string userId, int boardId, int listId, int id, CardRequest request)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return false;

        BoardList? list = await _db.BoardLists.FindAsync(listId);
        if (list == null || list.BoardId != boardId)
            return false;

        Card? card = await _db.Cards.FindAsync(id);
        if (card == null || card.BoardListId != listId)
            return false;

        card.Title = request.Title;
        card.Description = request.Description;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, int boardId, int listId, int id)
    {
        if (!await _db.Boards.AnyAsync(b => b.Id == boardId && b.UserId == userId))
            return false;

        BoardList? list = await _db.BoardLists.FindAsync(listId);
        if (list == null || list.BoardId != boardId)
            return false;

        Card? card = await _db.Cards.FindAsync(id);
        if (card == null || card.BoardListId != listId)
            return false;

        _db.Cards.Remove(card);
        await _db.SaveChangesAsync();
        return true;
    }
}
