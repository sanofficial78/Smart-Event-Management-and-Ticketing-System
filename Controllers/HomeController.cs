using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly IEventService _eventService;
    private readonly IAuthService _authService;

    public HomeController(IEventService eventService, IAuthService authService)
    {
        _eventService = eventService;
        _authService = authService;
    }

    public IActionResult Index()
    {
        ViewBag.IsMember = _authService.GetCurrentMember() != null;
        ViewBag.UpcomingEvents = _eventService.GetUpcomingEvents().Take(6).ToList();
        return View();
    }

    public IActionResult Privacy() => View();

    public IActionResult About() => View();

    public IActionResult Feedback() => RedirectToAction("Create", "Inquiries");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}
