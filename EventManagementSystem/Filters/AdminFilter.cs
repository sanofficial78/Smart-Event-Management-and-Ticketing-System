using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagementSystem.Filters;

public class AdminFilter : IActionFilter
{
    private readonly IAuthService _authService;

    public AdminFilter(IAuthService authService) => _authService = authService;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller c)
            c.ViewBag.IsAdmin = _authService.GetCurrentAdmin() != null;

        var action = context.RouteData.Values["action"]?.ToString();
        if (string.Equals(action, "Login", StringComparison.OrdinalIgnoreCase))
            return;

        if (_authService.GetCurrentAdmin() == null)
            context.Result = new RedirectToActionResult("Login", "Admin", new { returnUrl = context.HttpContext.Request.Path });
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
