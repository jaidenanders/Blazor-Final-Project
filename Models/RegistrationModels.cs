using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class RegistrationModel
{
    public int EventId { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 60 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "You may register between 1 and 5 tickets per submission.")]
    public int TicketCount { get; set; } = 1;

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the EventEase Terms of Service.")]
    public bool AgreeToTerms { get; set; } = false;

    public DateTime RegistrationTimestamp { get; set; } = DateTime.UtcNow;
}

public class UserSession
{
    public string SessionId { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string? CurrentUserEmail { get; set; }
    public string? CurrentUserName { get; set; }
    public DateTime SessionStarted { get; set; } = DateTime.UtcNow;
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(CurrentUserEmail);
}