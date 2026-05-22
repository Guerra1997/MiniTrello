using System.ComponentModel.DataAnnotations;

namespace MiniTrello.DTOs;

public class BoardListRequest
{
    [Required(ErrorMessage = "Board name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public required string Name { get; set; } = string.Empty;

}