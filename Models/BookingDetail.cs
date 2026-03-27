namespace EventManagementSystem.Models;

public class BookingDetail
{
    public int BookingDetailID { get; set; }
    public int BookingID { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }

    public Booking Booking { get; set; } = null!;
}
