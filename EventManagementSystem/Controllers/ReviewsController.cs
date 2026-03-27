using EventManagementSystem.Helpers;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IAuthService _authService;
    private readonly IEventService _eventService;

    public ReviewsController(IReviewService reviewService, IAuthService authService, IEventService eventService)
    {
        _reviewService = reviewService;
        _authService = authService;
        _eventService = eventService;
    }

    [HttpGet]
    public IActionResult Index(string? category, int? eventId)
    {
        var safeCategory = InputSanitizer.SanitizeSearchInput(category);
        var reviews = _reviewService.GetAllReviews(safeCategory, eventId);
        ViewBag.Categories = new[] { "Music", "Theatre", "Sports", "Arts", "Workshop", "Exhibition" };
        ViewBag.SelectedCategory = category;
        ViewBag.SelectedEventId = eventId;
        return View(reviews);
    }

    [HttpGet]
    public IActionResult MyReviews()
    {
        var member = _authService.GetCurrentMember();
        if (member == null)
        {
            TempData["Message"] = "Please sign in to view your reviews.";
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action(nameof(MyReviews)) });
        }
        var reviews = _reviewService.GetMemberReviews(member.MemberID);
        return View(reviews);
    }

    [HttpGet]
    public IActionResult Create(int eventId)
    {
        var member = _authService.GetCurrentMember();
        if (member == null)
        {
            TempData["Message"] = "Please sign in to submit a review.";
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Events", new { id = eventId }) });
        }
        var ev = _eventService.GetById(eventId);
        if (ev == null) return NotFound();
        if (_reviewService.HasReviewed(member.MemberID, eventId))
        {
            TempData["Message"] = "You have already reviewed this event.";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }
        return View(new Models.SubmitReviewViewModel { EventID = eventId, EventTitle = ev.Title });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Models.SubmitReviewViewModel model)
    {
        var member = _authService.GetCurrentMember();
        if (member == null) return RedirectToAction("Login", "Account");
        if (!ModelState.IsValid)
            return View(model);
        var (success, message) = _reviewService.SubmitReview(member.MemberID, model.EventID, model.Rating, model.Comment);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }
        TempData["Message"] = message;
        return RedirectToAction("Details", "Events", new { id = model.EventID });
    }
}
