using System.ComponentModel.DataAnnotations;

namespace MiniTrello.DTOs;

//Used to create and update
public class BoardRequest
{
    [Required(ErrorMessage = "Board name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
}