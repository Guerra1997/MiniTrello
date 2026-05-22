using MiniTrello.DTOs;

namespace MiniTrello.Services.Interfaces;

public interface IBoardListService
{
    Task<List<BoardListDto>?> GetAllAsync(string userId, int boardId);
    Task<BoardListDto?> GetByIdAsync(string userId, int boardId, int id);
    Task<BoardListDto?> CreateAsync(string userId, int boardId, BoardListRequest boardlist);
    Task<bool> UpdateAsync(string userId, int boardId, int id, BoardListRequest update);
    Task<bool> DeleteAsync(string userId, int boardId, int id);
}