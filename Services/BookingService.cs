using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db) => _db = db;

    public BookTicketsViewModel? GetBookingForm(int eventId)
    {
        var ev = _db.Events.Include(e => e.Venue).Include(e => e.EventSeats).FirstOrDefault(e => e.EventID == eventId);
        if (ev == null) return null;
        return new BookTicketsViewModel
        {
            EventID = ev.EventID,
            EventTitle = ev.Title,
            SeatOptions = ev.EventSeats.Where(s => s.AvailableSeats > 0).Select(s => new SeatOption
            {
                SeatType = s.SeatType,
                Price = s.Price,
                Available = s.AvailableSeats,
                Quantity = 0
            }).ToList()
        };
    }

    public (bool Success, string Message) CreateBooking(int memberId, int eventId, Dictionary<string, int> seatQuantities, bool simulatePaymentFailure = false)
    {
        var items = seatQuantities.Where(x => x.Value > 0).ToList();
        if (!items.Any()) return (false, "Please select at least one ticket.");
        var ev = _db.Events.Include(e => e.EventSeats).FirstOrDefault(e => e.EventID == eventId);
        if (ev == null) return (false, "Event not found.");
        decimal total = 0;
        var details = new List<BookingDetail>();
        foreach (var (seatType, qty) in items)
        {
            var es = ev.EventSeats.FirstOrDefault(s => s.SeatType == seatType && s.AvailableSeats >= qty);
            if (es == null) return (false, $"Not enough {seatType} seats available.");
            var subtotal = es.Price * qty;
            total += subtotal;
            details.Add(new BookingDetail { SeatType = seatType, Quantity = qty, UnitPrice = es.Price, SubTotal = subtotal });
            es.AvailableSeats -= qty;
        }
        var booking = new Booking
        {
            MemberID = memberId,
            EventID = eventId,
            Status = "Confirmed",
            TotalAmount = total,
            BookingDetails = details
        };
        _db.Bookings.Add(booking);

        using var transaction = _db.Database.BeginTransaction();
        try
        {
            _db.SaveChanges();

            if (simulatePaymentFailure)
            {
                transaction.Rollback();
                return (false, "Payment failed. No seats were reserved. Please try again or use a different payment method.");
            }

            transaction.Commit();
            return (true, "Tickets booked successfully!");
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Booking> GetMemberBookings(int memberId)
    {
        return _db.Bookings
            .Include(b => b.Event).ThenInclude(e => e!.Venue)
            .Include(b => b.BookingDetails)
            .Where(b => b.MemberID == memberId)
            .OrderByDescending(b => b.BookingDate)
            .ToList();
    }

    public bool HasBooking(int memberId, int eventId)
    {
        return _db.Bookings.Any(b => b.MemberID == memberId && b.EventID == eventId && b.Status != "Cancelled");
    }
}
