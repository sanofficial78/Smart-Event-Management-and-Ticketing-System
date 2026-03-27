using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IAuthService _authService;
    private readonly IEventService _eventService;

    public BookingsController(IBookingService bookingService, IAuthService authService, IEventService eventService)
    {
        _bookingService = bookingService;
        _authService = authService;
        _eventService = eventService;
    }

    private ActionResult? RequireMember()
    {
        var member = _authService.GetCurrentMember();
        if (member == null)
        {
            TempData["Message"] = "Please sign in to book tickets.";
            return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
        }
        return null;
    }

    [HttpGet]
    public IActionResult Book(int id)
    {
        var check = RequireMember();
        if (check != null) return check;
        var form = _bookingService.GetBookingForm(id);
        if (form == null) return NotFound();
        if (!form.SeatOptions.Any())
        {
            TempData["Message"] = "No tickets available for this event.";
            return RedirectToAction("Details", "Events", new { id });
        }
        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Book(int eventId, Dictionary<string, int> seats, bool simulatePaymentFailure = false)
    {
        var check = RequireMember();
        if (check != null) return check;
        var member = _authService.GetCurrentMember()!;
        var (success, message) = _bookingService.CreateBooking(member.MemberID, eventId, seats, simulatePaymentFailure);
        TempData["Message"] = message;
        if (success)
            return RedirectToAction(nameof(MyBookings));
        return RedirectToAction(nameof(Book), new { id = eventId });
    }

    public IActionResult MyBookings()
    {
        var check = RequireMember();
        if (check != null) return check;
        var member = _authService.GetCurrentMember()!;
        var bookings = _bookingService.GetMemberBookings(member.MemberID);
        return View(bookings);
    }
}
