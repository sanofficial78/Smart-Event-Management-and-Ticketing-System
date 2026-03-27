using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _http;

    public AuthService(AppDbContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    public (bool Success, string Message) Register(RegisterViewModel model)
    {
        if (_db.Members.Any(m => m.Email == model.Email))
            return (false, "Email already registered.");
        var hash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        var member = new Member
        {
            Email = model.Email,
            PasswordHash = hash,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Address = model.Address,
            PreferredMusic = model.PreferredMusic,
            PreferredTheatre = model.PreferredTheatre,
            PreferredSports = model.PreferredSports,
            PreferredArts = model.PreferredArts,
            PreferredWorkshops = model.PreferredWorkshops,
            PreferredExhibitions = model.PreferredExhibitions
        };
        _db.Members.Add(member);
        _db.SaveChanges();
        return (true, "Registration successful. Please sign in.");
    }

    public (bool Success, string Message, Member? Member) Login(string email, string password)
    {
        var member = _db.Members.FirstOrDefault(m => m.Email == email && m.IsActive);
        if (member == null) return (false, "Invalid email or password.", null);
        if (!BCrypt.Net.BCrypt.Verify(password, member.PasswordHash))
            return (false, "Invalid email or password.", null);
        return (true, "Login successful.", member);
    }

    public (bool Success, string Message, Member? Member) AdminLogin(string email, string password)
    {
        var (success, message, member) = Login(email, password);
        if (!success || member == null) return (success, message, null);
        if (!member.IsAdmin) return (false, "Access denied. Admin credentials required.", null);
        return (true, "Welcome back, Admin.", member);
    }

    public void Logout()
    {
        _http.HttpContext?.Session.Remove("MemberID");
    }

    public Member? GetCurrentMember()
    {
        var id = _http.HttpContext?.Session.GetInt32("MemberID");
        if (id == null) return null;
        return _db.Members.AsNoTracking().FirstOrDefault(m => m.MemberID == id);
    }

    public Member? GetCurrentAdmin()
    {
        var member = GetCurrentMember();
        return member != null && member.IsAdmin ? member : null;
    }
}
