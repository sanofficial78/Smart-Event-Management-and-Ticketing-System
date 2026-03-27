namespace EventManagementSystem.Models;

public class Booking
{
    public int BookingID { get; set; }
    public int MemberID { get; set; }
    public int EventID { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Confirmed";
    public decimal TotalAmount { get; set; }

    public Member Member { get; set; } = null!;
    public Event Event { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}
