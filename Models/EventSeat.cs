namespace EventManagementSystem.Models;

public class EventSeat
{
    public int EventSeatID { get; set; }
    public int EventID { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }

    public Event Event { get; set; } = null!;
}
