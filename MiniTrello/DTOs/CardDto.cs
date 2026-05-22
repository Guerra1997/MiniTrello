namespace MiniTrello.DTOs;

public class CardDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int BoardListId { get; set; }
    public DateTime CreatedAt { get; set; }
}