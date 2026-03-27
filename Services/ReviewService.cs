using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;

    public ReviewService(AppDbContext db) => _db = db;

    public List<Review> GetEventReviews(int eventId)
    {
        return _db.Reviews
            .Include(r => r.Member)
            .Where(r => r.EventID == eventId)
            .OrderByDescending(r => r.ReviewDate)
            .ToList();
    }

    public List<Review> GetMemberReviews(int memberId)
    {
        return _db.Reviews
            .Include(r => r.Event)
            .Where(r => r.MemberID == memberId)
            .OrderByDescending(r => r.ReviewDate)
            .ToList();
    }

    public List<Review> GetAllReviews(string? category = null, int? eventId = null)
    {
        var q = _db.Reviews
            .Include(r => r.Member)
            .Include(r => r.Event).ThenInclude(e => e!.Venue)
            .AsQueryable();
        if (eventId.HasValue)
            q = q.Where(r => r.EventID == eventId.Value);
        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(r => r.Event!.Category == category);
        return q.OrderByDescending(r => r.ReviewDate).ToList();
    }

    public (bool Success, string Message) SubmitReview(int memberId, int eventId, int rating, string? comment)
    {
        if (rating < 1 || rating > 5) return (false, "Validation error (rating must be 1-5).");
        if (_db.Reviews.Any(r => r.MemberID == memberId && r.EventID == eventId))
            return (false, "You have already reviewed this event.");
        _db.Reviews.Add(new Review { MemberID = memberId, EventID = eventId, Rating = rating, Comment = comment });
        _db.SaveChanges();
        return (true, "Review submitted. Thank you!");
    }

    public bool HasReviewed(int memberId, int eventId)
    {
        return _db.Reviews.Any(r => r.MemberID == memberId && r.EventID == eventId);
    }
}
