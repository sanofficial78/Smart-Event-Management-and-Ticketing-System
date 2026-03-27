using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagementSystem.Filters;

public class MemberFilter : IActionFilter
{
    private readonly IAuthService _authService;

    public MemberFilter(IAuthService authService) => _authService = authService;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is Controller c)
        {
            c.ViewBag.IsMember = _authService.GetCurrentMember() != null;
            c.ViewBag.IsAdmin = _authService.GetCurrentAdmin() != null;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
