using Microsoft.EntityFrameworkCore;
using MiniTrello.Data;
using MiniTrello.DTOs;
using MiniTrello.Models;
using MiniTrello.Services;

namespace MiniTrello.Tests;

public class BoardListServiceTests
{
    private MiniTrelloDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<MiniTrelloDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MiniTrelloDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyCurrentUserBoardLists()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardA = db.Boards.First(b => b.UserId == "user1");
        var boardB = db.Boards.First(b => b.UserId == "user2");

        db.BoardLists.AddRange(
            new BoardList { Name = "Board A", BoardId = boardA.Id, CreatedAt = DateTime.UtcNow },
            new BoardList { Name = "Board B", BoardId = boardB.Id, CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var service = new BoardListService(db);

        // Act
        var result = await service.GetAllAsync("user1", boardA.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, b => Assert.Equal(boardA.Id, b.BoardId));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenBoardBelongsToDifferentUser()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardUser1 = db.Boards.First(b => b.UserId == "user1");
        var boardUser2 = db.Boards.First(b => b.UserId == "user2");

        db.BoardLists.AddRange(
            new BoardList { Name = "Board A", BoardId = boardUser1.Id, CreatedAt = DateTime.UtcNow },
            new BoardList { Name = "Board B", BoardId = boardUser2.Id, CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardListA = db.BoardLists.First(b => b.BoardId == boardUser1.Id);
        var service = new BoardListService(db);

        // Act
        var result = await service.GetByIdAsync("user1", boardUser2.Id, boardListA.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreateListWithCorrectBoard()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardUser1 = db.Boards.First(b => b.UserId == "user1");

        var service = new BoardListService(db);

        // Act
        BoardListRequest request = new BoardListRequest
        {
            Name = "BoardList"
        };
        var result = await service.CreateAsync("user1", boardUser1.Id, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(boardUser1.Id, result.BoardId);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseForWrongBoard()
    {
        // Arrange
        using var db = CreateDb();
        db.Boards.AddRange(
            new Board { UserId = "user1", Name = "Board A", CreatedAt = DateTime.UtcNow },
            new Board { UserId = "user2", Name = "Board C", CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardUser1 = db.Boards.First(b => b.UserId == "user1");
        var boardUser2 = db.Boards.First(b => b.UserId == "user2");

        db.BoardLists.AddRange(
            new BoardList { Name = "Board A", BoardId = boardUser1.Id, CreatedAt = DateTime.UtcNow },
            new BoardList { Name = "Board B", BoardId = boardUser2.Id, CreatedAt = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
        var boardListA = db.BoardLists.First(b => b.BoardId == boardUser1.Id);
        var service = new BoardListService(db);

        // Act
        var result = await service.DeleteAsync("user1", boardUser2.Id, boardListA.Id);

        // Assert
        Assert.False(result);
    }
}