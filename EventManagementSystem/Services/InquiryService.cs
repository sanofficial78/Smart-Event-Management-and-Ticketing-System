using EventManagementSystem.Data;
using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public class InquiryService : IInquiryService
{
    private readonly AppDbContext _db;

    public InquiryService(AppDbContext db) => _db = db;

    public (bool Success, string Message) Submit(InquiryViewModel model)
    {
        _db.Inquiries.Add(new Inquiry
        {
            GuestName = model.GuestName,
            GuestEmail = model.GuestEmail,
            Subject = model.Subject,
            Message = model.Message
        });
        _db.SaveChanges();
        return (true, "Your inquiry has been submitted. We will respond shortly.");
    }
}
