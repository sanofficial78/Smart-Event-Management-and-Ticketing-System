using EventManagementSystem.Data;
using EventManagementSystem.Filters;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Controllers;

[ServiceFilter(typeof(AdminFilter))]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly IAuthService _authService;

    public AdminController(AppDbContext db, IAuthService authService)
    {
        _db = db;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (_authService.GetCurrentAdmin() != null)
            return Redirect(returnUrl ?? Url.Action(nameof(Dashboard)) ?? "/Admin");
        ViewBag.ReturnUrl = returnUrl ?? Url.Action(nameof(Dashboard));
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl ?? Url.Action(nameof(Dashboard));
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return View(model);
        }
        var (success, message, member) = _authService.AdminLogin(model.Email, model.Password);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }
        HttpContext.Session.SetInt32("MemberID", member!.MemberID);
        return Redirect(returnUrl ?? Url.Action(nameof(Dashboard)) ?? "/Admin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Login");
    }

    public IActionResult Index() => RedirectToAction(nameof(Dashboard));

    public IActionResult Dashboard()
    {
        var now = DateTime.UtcNow;
        ViewBag.TotalEvents = _db.Events.Count();
        ViewBag.UpcomingEvents = _db.Events.Count(e => e.IsActive && e.EventDate >= now);
        ViewBag.TotalBookings = _db.Bookings.Count();
        ViewBag.TotalRevenue = _db.Bookings.Where(b => b.Status != "Cancelled").Sum(b => b.TotalAmount);
        ViewBag.TotalMembers = _db.Members.Count(m => !m.IsAdmin);
        ViewBag.TotalInquiries = _db.Inquiries.Count();
        ViewBag.UnresolvedInquiries = _db.Inquiries.Count(i => !i.IsResolved);
        ViewBag.TotalVenues = _db.Venues.Count();

        var recentBookings = _db.Bookings
            .Include(b => b.Member)
            .Include(b => b.Event)
            .OrderByDescending(b => b.BookingDate)
            .Take(8)
            .ToList();
        ViewBag.RecentBookings = recentBookings;

        var recentInquiries = _db.Inquiries
            .OrderByDescending(i => i.InquiryDate)
            .Take(5)
            .ToList();
        ViewBag.RecentInquiries = recentInquiries;

        return View();
    }

    public IActionResult Events()
    {
        var events = _db.Events
            .Include(e => e.Venue)
            .OrderByDescending(e => e.EventDate)
            .ToList();
        return View(events);
    }

    public IActionResult Bookings()
    {
        var bookings = _db.Bookings
            .Include(b => b.Member)
            .Include(b => b.Event)
            .OrderByDescending(b => b.BookingDate)
            .ToList();
        return View(bookings);
    }

    public IActionResult Members()
    {
        var members = _db.Members
            .Where(m => !m.IsAdmin)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();
        return View(members);
    }

    public IActionResult Inquiries()
    {
        var inquiries = _db.Inquiries
            .OrderByDescending(i => i.InquiryDate)
            .ToList();
        return View(inquiries);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResolveInquiry(int id)
    {
        var inquiry = _db.Inquiries.Find(id);
        if (inquiry != null)
        {
            inquiry.IsResolved = true;
            _db.SaveChanges();
            TempData["Message"] = "Inquiry marked as resolved.";
        }
        return RedirectToAction(nameof(Inquiries));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RejectInquiry(int id)
    {
        var inquiry = _db.Inquiries.Find(id);
        if (inquiry != null)
        {
            inquiry.IsResolved = true; // Treat reject as closed
            _db.SaveChanges();
            TempData["Message"] = "Inquiry rejected/closed.";
        }
        return RedirectToAction(nameof(Inquiries));
    }

    public IActionResult InquiryDetails(int id)
    {
        var inquiry = _db.Inquiries.Find(id);
        if (inquiry == null) return NotFound();
        return View(inquiry);
    }

    // --- Bookings ---
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmBooking(int id)
    {
        var booking = _db.Bookings.Find(id);
        if (booking != null)
        {
            booking.Status = "Confirmed";
            _db.SaveChanges();
            TempData["Message"] = "Booking confirmed.";
        }
        return RedirectToAction(nameof(Bookings));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelBooking(int id)
    {
        var booking = _db.Bookings.Include(b => b.BookingDetails).FirstOrDefault(b => b.BookingID == id);
        if (booking != null && booking.Status != "Cancelled")
        {
            foreach (var d in booking.BookingDetails)
            {
                var seat = _db.EventSeats.FirstOrDefault(s => s.EventID == booking.EventID && s.SeatType == d.SeatType);
                if (seat != null) seat.AvailableSeats += d.Quantity;
            }
            booking.Status = "Cancelled";
            _db.SaveChanges();
            TempData["Message"] = "Booking cancelled. Seats released.";
        }
        return RedirectToAction(nameof(Bookings));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CompleteBooking(int id)
    {
        var booking = _db.Bookings.Find(id);
        if (booking != null)
        {
            booking.Status = "Completed";
            _db.SaveChanges();
            TempData["Message"] = "Booking marked as completed.";
        }
        return RedirectToAction(nameof(Bookings));
    }

    public IActionResult BookingDetails(int id)
    {
        var booking = _db.Bookings
            .Include(b => b.Member)
            .Include(b => b.Event).ThenInclude(e => e!.Venue)
            .Include(b => b.BookingDetails)
            .FirstOrDefault(b => b.BookingID == id);
        if (booking == null) return NotFound();
        return View(booking);
    }

    // --- Members ---
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleMemberStatus(int id)
    {
        var member = _db.Members.Find(id);
        if (member != null && !member.IsAdmin)
        {
            member.IsActive = !member.IsActive;
            _db.SaveChanges();
            TempData["Message"] = $"Member {(member.IsActive ? "activated" : "deactivated")}.";
        }
        return RedirectToAction(nameof(Members));
    }

    public IActionResult MemberDetails(int id)
    {
        var member = _db.Members
            .Include(m => m.Bookings).ThenInclude(b => b.Event)
            .FirstOrDefault(m => m.MemberID == id);
        if (member == null || member.IsAdmin) return NotFound();
        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteMember(int id)
    {
        var member = _db.Members.Find(id);
        if (member == null || member.IsAdmin)
            return RedirectToAction(nameof(Members));
        var hasBookings = _db.Bookings.Any(b => b.MemberID == id);
        var hasReviews = _db.Reviews.Any(r => r.MemberID == id);
        if (hasBookings || hasReviews)
        {
            TempData["Message"] = "FK constraint error (prevent delete). Cannot delete member: member has bookings or reviews. Deactivate the member instead.";
            return RedirectToAction(nameof(Members));
        }
        _db.Members.Remove(member);
        _db.SaveChanges();
        TempData["Message"] = "Member deleted successfully.";
        return RedirectToAction(nameof(Members));
    }

    // --- Events ---
    [HttpGet]
    public IActionResult AddEvent()
    {
        ViewBag.Venues = _db.Venues.OrderBy(v => v.Name).ToList();
        return View(new AdminEventEditViewModel { EventDate = DateTime.UtcNow.AddDays(7) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddEvent(AdminEventEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var venue = _db.Venues.Find(model.VenueID);
            var ev = new Event
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                EventDate = model.EventDate,
                VenueID = model.VenueID,
                BasePrice = model.BasePrice,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive
            };
            _db.Events.Add(ev);
            _db.SaveChanges();
            var cap = venue?.Capacity ?? 100;
            _db.EventSeats.Add(new EventSeat { EventID = ev.EventID, SeatType = "General", Price = model.BasePrice, TotalSeats = cap, AvailableSeats = cap });
            _db.SaveChanges();
            TempData["Message"] = "Event created successfully with default seating.";
            return RedirectToAction(nameof(Events));
        }
        ViewBag.Venues = _db.Venues.OrderBy(v => v.Name).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult EditEvent(int id)
    {
        var ev = _db.Events.Find(id);
        if (ev == null) return NotFound();
        ViewBag.Venues = _db.Venues.OrderBy(v => v.Name).ToList();
        return View(new AdminEventEditViewModel
        {
            EventID = ev.EventID,
            Title = ev.Title,
            Description = ev.Description,
            Category = ev.Category,
            EventDate = ev.EventDate,
            VenueID = ev.VenueID,
            BasePrice = ev.BasePrice,
            ImageUrl = ev.ImageUrl,
            IsActive = ev.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditEvent(AdminEventEditViewModel model)
    {
        var ev = _db.Events.Find(model.EventID);
        if (ev == null) return NotFound();
        if (ModelState.IsValid)
        {
            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.Category = model.Category;
            ev.EventDate = model.EventDate;
            ev.VenueID = model.VenueID;
            ev.BasePrice = model.BasePrice;
            ev.ImageUrl = model.ImageUrl;
            ev.IsActive = model.IsActive;
            _db.SaveChanges();
            TempData["Message"] = "Event updated successfully.";
            return RedirectToAction(nameof(Events));
        }
        ViewBag.Venues = _db.Venues.OrderBy(v => v.Name).ToList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEvent(int id)
    {
        var ev = _db.Events.Include(e => e.EventSeats).FirstOrDefault(e => e.EventID == id);
        if (ev != null)
        {
            _db.Events.Remove(ev);
            _db.SaveChanges();
            TempData["Message"] = "Event deleted.";
        }
        return RedirectToAction(nameof(Events));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleEventActive(int id)
    {
        var ev = _db.Events.Find(id);
        if (ev != null)
        {
            ev.IsActive = !ev.IsActive;
            _db.SaveChanges();
            TempData["Message"] = $"Event {(ev.IsActive ? "activated" : "deactivated")}.";
        }
        return RedirectToAction(nameof(Events));
    }

    // --- Venues ---
    [HttpGet]
    public IActionResult AddVenue()
    {
        return View(new AdminVenueEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddVenue(AdminVenueEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            _db.Venues.Add(new Venue
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                Capacity = model.Capacity,
                Description = model.Description
            });
            _db.SaveChanges();
            TempData["Message"] = "Venue created successfully.";
            return RedirectToAction(nameof(Venues));
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult EditVenue(int id)
    {
        var v = _db.Venues.Find(id);
        if (v == null) return NotFound();
        return View(new AdminVenueEditViewModel
        {
            VenueID = v.VenueID,
            Name = v.Name,
            Address = v.Address,
            City = v.City,
            Capacity = v.Capacity,
            Description = v.Description
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditVenue(AdminVenueEditViewModel model)
    {
        var v = _db.Venues.Find(model.VenueID);
        if (v == null) return NotFound();
        if (ModelState.IsValid)
        {
            v.Name = model.Name;
            v.Address = model.Address;
            v.City = model.City;
            v.Capacity = model.Capacity;
            v.Description = model.Description;
            _db.SaveChanges();
            TempData["Message"] = "Venue updated successfully.";
            return RedirectToAction(nameof(Venues));
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteVenue(int id)
    {
        var v = _db.Venues.Include(x => x.Events).FirstOrDefault(x => x.VenueID == id);
        if (v != null)
        {
            if (v.Events.Any())
            {
                TempData["Message"] = "Cannot delete venue with existing events.";
            }
            else
            {
                _db.Venues.Remove(v);
                _db.SaveChanges();
                TempData["Message"] = "Venue deleted.";
            }
        }
        return RedirectToAction(nameof(Venues));
    }

    public IActionResult Venues()
    {
        var venues = _db.Venues.Include(v => v.Events).OrderBy(v => v.Name).ToList();
        return View(venues);
    }
}
