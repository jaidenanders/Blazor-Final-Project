using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class EventItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 150 characters.")]
    public string Location { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;
}