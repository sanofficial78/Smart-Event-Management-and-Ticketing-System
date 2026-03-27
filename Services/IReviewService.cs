using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public interface IReviewService
{
    List<Review> GetEventReviews(int eventId);
    List<Review> GetMemberReviews(int memberId);
    List<Review> GetAllReviews(string? category = null, int? eventId = null);
    (bool Success, string Message) SubmitReview(int memberId, int eventId, int rating, string? comment);
    bool HasReviewed(int memberId, int eventId);
}
