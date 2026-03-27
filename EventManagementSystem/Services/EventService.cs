using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;

    public EventService(AppDbContext db) => _db = db;

    public IQueryable<Event> GetUpcomingEvents()
    {
        return _db.Events
            .Include(e => e.Venue)
            .Where(e => e.IsActive && e.EventDate >= DateTime.UtcNow)
            .OrderBy(e => e.EventDate);
    }

    public Event? GetById(int id)
    {
        return _db.Events
            .Include(e => e.Venue)
            .Include(e => e.EventSeats)
            .Include(e => e.Reviews).ThenInclude(r => r.Member)
            .FirstOrDefault(e => e.EventID == id);
    }

    public EventSearchResult Search(string? category, DateTime? from, DateTime? to, string? city, decimal? minPrice, decimal? maxPrice, bool guestView = false)
    {
        var q = _db.Events.Include(e => e.Venue).Where(e => e.IsActive && e.EventDate >= DateTime.UtcNow);
        if (!string.IsNullOrWhiteSpace(category)) q = q.Where(e => e.Category == category);
        if (from.HasValue) q = q.Where(e => e.EventDate >= from);
        if (to.HasValue) q = q.Where(e => e.EventDate <= to);
        if (!string.IsNullOrWhiteSpace(city)) q = q.Where(e => e.Venue.City == city);
        if (minPrice.HasValue) q = q.Where(e => e.BasePrice >= minPrice);
        if (maxPrice.HasValue) q = q.Where(e => e.BasePrice <= maxPrice);
        var events = q.OrderBy(e => e.EventDate).ToList();
        var results = events.Select(e => new EventListItem
        {
            EventID = e.EventID,
            Title = e.Title,
            Category = e.Category,
            EventDate = e.EventDate,
            VenueName = e.Venue?.Name ?? "",
            City = e.Venue?.City ?? "",
            BasePrice = e.BasePrice,
            Availability = guestView ? (e.EventSeats?.Any(s => s.AvailableSeats > 0) == true ? "Available" : "Full") : null
        }).ToList();
        if (guestView && events.Any())
        {
            var ids = events.Select(e => e.EventID).ToList();
            var seats = _db.EventSeats.Where(s => ids.Contains(s.EventID)).ToList();
            foreach (var r in results)
            {
                var hasAvail = seats.Any(s => s.EventID == r.EventID && s.AvailableSeats > 0);
                r.Availability = hasAvail ? "Available" : "Full";
            }
        }
        return new EventSearchResult { Events = results };
    }

    public (bool Available, string Status) GetAvailability(int eventId)
    {
        var any = _db.EventSeats.Any(s => s.EventID == eventId && s.AvailableSeats > 0);
        return (any, any ? "Available" : "Full");
    }
}
