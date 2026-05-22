using MiniTrello.DTOs;

namespace MiniTrello.Services.Interfaces;

public interface IBoardService
{
    Task<List<BoardDto>> GetAllAsync(string userId);
    Task<BoardDto?> GetByIdAsync(string userId, int id);
    Task<BoardDto> CreateAsync(string userId, BoardRequest request);
    Task<bool> UpdateAsync(string userId, int id, BoardRequest request);
    Task<bool> DeleteAsync(string userId, int id);
}