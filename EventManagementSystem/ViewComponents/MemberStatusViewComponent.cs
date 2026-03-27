using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystem.ViewComponents;

public class MemberStatusViewComponent : ViewComponent
{
    private readonly IAuthService _authService;

    public MemberStatusViewComponent(IAuthService authService) => _authService = authService;

    public IViewComponentResult Invoke()
    {
        ViewBag.IsMember = _authService.GetCurrentMember() != null;
        return View();
    }
}
