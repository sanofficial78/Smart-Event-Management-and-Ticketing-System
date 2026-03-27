using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public interface IAuthService
{
    (bool Success, string Message) Register(RegisterViewModel model);
    (bool Success, string Message, Member? Member) Login(string email, string password);
    (bool Success, string Message, Member? Member) AdminLogin(string email, string password);
    void Logout();
    Member? GetCurrentMember();
    Member? GetCurrentAdmin();
}
