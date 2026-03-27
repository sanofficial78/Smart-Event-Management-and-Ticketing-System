namespace EventManagementSystem.Models;

public class Member
{
    public int MemberID { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; } = false;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
