namespace EventManagementSystem.Models;

public class RegisterViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool PreferredMusic { get; set; }
    public bool PreferredTheatre { get; set; }
    public bool PreferredSports { get; set; }
    public bool PreferredArts { get; set; }
    public bool PreferredWorkshops { get; set; }
    public bool PreferredExhibitions { get; set; }
}

public class LoginViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class EventListItem
{
    public int EventID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string? Availability { get; set; }
}

public class EventSearchResult
{
    public List<EventListItem> Events { get; set; } = new();
}

public class BookTicketsViewModel
{
    public int EventID { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public List<SeatOption> SeatOptions { get; set; } = new();
}

public class SeatOption
{
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Available { get; set; }
    public int Quantity { get; set; }
}

public class SubmitReviewViewModel
{
    public int EventID { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Range(1, 5, ErrorMessage = "Validation error (rating must be 1-5).")]
    public int Rating { get; set; }
    [System.ComponentModel.DataAnnotations.MaxLength(2000)]
    public string? Comment { get; set; }
}

public class InquiryViewModel
{
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class AdminEventEditViewModel
{
    public int EventID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int VenueID { get; set; }
    public decimal BasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

public class AdminVenueEditViewModel
{
    public int VenueID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Description { get; set; }
}
