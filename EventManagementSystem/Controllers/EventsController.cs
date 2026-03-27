using EventManagementSystem.Helpers;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;
    private readonly IAuthService _authService;
    private readonly IReviewService _reviewService;
    private readonly IBookingService _bookingService;

    public EventsController(IEventService eventService, IAuthService authService, IReviewService reviewService, IBookingService bookingService)
    {
        _eventService = eventService;
        _authService = authService;
        _reviewService = reviewService;
        _bookingService = bookingService;
    }

    private bool IsMember => _authService.GetCurrentMember() != null;

    public IActionResult Index(string? category, DateTime? from, DateTime? to, string? city, decimal? minPrice, decimal? maxPrice)
    {
        // Sanitize search inputs to prevent SQL injection (defense-in-depth; EF uses parameterized queries)
        var safeCategory = InputSanitizer.SanitizeSearchInput(category);
        var safeCity = InputSanitizer.SanitizeSearchInput(city);
        var result = _eventService.Search(safeCategory, from, to, safeCity, CurrencyHelper.LkrToBase(minPrice), CurrencyHelper.LkrToBase(maxPrice), guestView: !IsMember);
        ViewBag.IsMember = IsMember;
        ViewBag.Categories = new[] { "Music", "Theatre", "Sports", "Arts", "Workshop", "Exhibition" };
        return View(result);
    }

    public IActionResult Details(int id)
    {
        var ev = _eventService.GetById(id);
        if (ev == null) return NotFound();
        ViewBag.IsMember = IsMember;
        ViewBag.Reviews = _reviewService.GetEventReviews(id);
        if (IsMember)
        {
            var member = _authService.GetCurrentMember();
            if (member != null)
            {
                ViewBag.HasReviewed = _reviewService.HasReviewed(member.MemberID, id);
                ViewBag.HasBooking = _bookingService.HasBooking(member.MemberID, id);
            }
        }
        return View(ev);
    }
}
