using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public interface IInquiryService
{
    (bool Success, string Message) Submit(InquiryViewModel model);
}
