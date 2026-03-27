using EventManagementSystem.Models;

namespace EventManagementSystem.Services;

public interface IEventService
{
    IQueryable<Event> GetUpcomingEvents();
    Event? GetById(int id);
    EventSearchResult Search(string? category, DateTime? from, DateTime? to, string? city, decimal? minPrice, decimal? maxPrice, bool guestView = false);
    (bool Available, string Status) GetAvailability(int eventId);
}
