using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService) => _authService = authService;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Home");
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return View(model);
        }
        var (success, message, member) = _authService.Login(model.Email, model.Password);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }
        HttpContext.Session.SetInt32("MemberID", member!.MemberID);
        return Redirect(returnUrl ?? Url.Action("Index", "Home") ?? "/");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (model.Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
        if (!ModelState.IsValid) return View(model);
        var (success, message) = _authService.Register(model);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }
        TempData["Message"] = message;
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        _authService.Logout();
        return RedirectToAction("Index", "Home");
    }
}
