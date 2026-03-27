using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class InquiriesController : Controller
{
    private readonly IInquiryService _inquiryService;

    public InquiriesController(IInquiryService inquiryService) => _inquiryService = inquiryService;

    [HttpGet]
    public IActionResult Create() => View(new InquiryViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(InquiryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, message) = _inquiryService.Submit(model);
        TempData["Message"] = message;
        return RedirectToAction("Create");
    }
}
