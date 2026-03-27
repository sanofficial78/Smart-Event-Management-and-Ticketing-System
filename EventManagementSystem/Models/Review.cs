namespace EventManagementSystem.Models;

public class Review
{
    public int ReviewID { get; set; }
    public int MemberID { get; set; }
    public int EventID { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

    public Member Member { get; set; } = null!;
    public Event Event { get; set; } = null!;
}
