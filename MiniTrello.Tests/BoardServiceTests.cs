using Microsoft.EntityFrameworkCore;
using MiniTrello.Data;
using MiniTrello.DTOs;
using MiniTrello.Models;
using MiniTrello.Services;

namespace MiniTrello.Tests;

public class BoardServiceTests
{
    private MiniTrelloDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<MiniTrelloDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MiniTrelloDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyCurrentUserBoards()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user1", Name = "Board B", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();

        var service = new BoardService(db);

        // Act
        var result = await service.GetAllAsync("user1");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Equal("user1", b.UserId));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullForBoardDifferentUser()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user1", Name = "Board B", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardC = db.Boards.First(b => b.UserId == "user2");

        var service = new BoardService(db);

        // Act
        var result = await service.GetByIdAsync("user1", boardC.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreateBoardWithCorrectUser()
    {
        // Arrange
        using var db = CreateDb();
        var service = new BoardService(db);

        // Act
        BoardRequest request = new BoardRequest
        {
            Name = "Name",
            Description = "Description"
        };
        var result = await service.CreateAsync("user1", request);

        // Assert
        Assert.Equal("user1", result.UserId);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseForOtherUserBoard()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user1", Name = "Board B", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();

        var service = new BoardService(db);

        // Act
        BoardRequest request = new BoardRequest
        {
            Name = "Name",
            Description = "Description"
        };
        var result = await service.DeleteAsync("user1", 3);

        // Assert
        Assert.False(result);
    }
}
