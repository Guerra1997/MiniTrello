namespace MiniTrello.DTOs;

public class BoardListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public List<CardDto> Cards { get; set; } = [];
    public DateTime CreatedAt { get; set; }

}