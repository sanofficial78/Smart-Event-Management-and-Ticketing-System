using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public interface IBookingService
{
    BookTicketsViewModel? GetBookingForm(int eventId);
    (bool Success, string Message) CreateBooking(int memberId, int eventId, Dictionary<string, int> seatQuantities, bool simulatePaymentFailure = false);
    List<Booking> GetMemberBookings(int memberId);
    bool HasBooking(int memberId, int eventId);
}
