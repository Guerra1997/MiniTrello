using MiniTrello.DTOs;

namespace MiniTrello.Services.Interfaces;

public interface ICardService
{
    Task<List<CardDto>?> GetAllAsync(string userId, int boardId, int listId);
    Task<CardDto?> GetByIdAsync(string userId, int boardId, int listId, int id);
    Task<CardDto?> CreateAsync(string userId, int boardId, int listId, CardRequest request);
    Task<bool> UpdateAsync(string userId, int boardId, int listId, int id, CardRequest request);
    Task<bool> DeleteAsync(string userId, int boardId, int listId, int id);
}
