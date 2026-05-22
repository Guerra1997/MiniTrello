namespace MiniTrello.Models;

public class BoardList
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    public List<Card> Cards { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}