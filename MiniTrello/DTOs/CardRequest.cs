using System.ComponentModel.DataAnnotations;

namespace MiniTrello.DTOs;

public class CardRequest
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tile must be between 3 and 50 characters")]
    public required string Title { get; set; }
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
}