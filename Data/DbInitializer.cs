using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        EnsureIsAdminColumn(db);
        EnsureAdminExists(db);

        if (db.Venues.Any()) return;

        // Test member password: Member123!
        var pwHash = BCrypt.Net.BCrypt.HashPassword("Member123!");

        var venues = new List<Venue>
        {
            new() { Name = "Metropolitan Concert Hall", Address = "123 Arts Boulevard", City = "Metro City", Capacity = 2000, Description = "Premier concert venue" },
            new() { Name = "Community Theatre", Address = "456 Drama Street", City = "Metro City", Capacity = 500, Description = "Intimate theatre" },
            new() { Name = "City Exhibition Center", Address = "789 Gallery Way", City = "Metro City", Capacity = 1000, Description = "Spacious galleries" },
            new() { Name = "Workshop Studio", Address = "321 Creative Lane", City = "Metro City", Capacity = 50, Description = "Hands-on workshop space" }
        };
        db.Venues.AddRange(venues);
        db.SaveChanges();

        var events = new List<Event>
        {
            new() { Title = "Summer Jazz Festival 2025", Description = "Three days of jazz performances.", Category = "Music", EventDate = DateTime.UtcNow.AddMonths(2), VenueID = 1, BasePrice = 45 },
            new() { Title = "Romeo and Juliet", Description = "Classic Shakespeare play.", Category = "Theatre", EventDate = DateTime.UtcNow.AddMonths(1), VenueID = 2, BasePrice = 35 },
            new() { Title = "Modern Art Exhibition", Description = "Contemporary art from emerging artists.", Category = "Exhibition", EventDate = DateTime.UtcNow.AddDays(20), VenueID = 3, BasePrice = 15 },
            new() { Title = "Pottery Making Workshop", Description = "Learn the basics of pottery.", Category = "Workshop", EventDate = DateTime.UtcNow.AddDays(30), VenueID = 4, BasePrice = 75 },
            new() { Title = "Symphony Orchestra Night", Description = "Beethoven and Mozart.", Category = "Music", EventDate = DateTime.UtcNow.AddMonths(1).AddDays(5), VenueID = 1, BasePrice = 55 }
        };
        db.Events.AddRange(events);
        db.SaveChanges();

        var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        var members = new List<Member>
        {
            new() { Email = "admin@metro.gov", PasswordHash = adminHash, FirstName = "Admin", LastName = "User", IsAdmin = true },
            new() { Email = "john@test.com", PasswordHash = pwHash, FirstName = "John", LastName = "Doe", PreferredMusic = true, PreferredTheatre = true },
            new() { Email = "jane@test.com", PasswordHash = pwHash, FirstName = "Jane", LastName = "Smith", PreferredMusic = true, PreferredArts = true }
        };
        db.Members.AddRange(members);
        db.SaveChanges();

        var seatData = new[] {
            (1, "VIP", 120m, 100), (1, "Standard", 45m, 1500), (1, "Balcony", 30m, 400),
            (2, "VIP", 80m, 50), (2, "Standard", 35m, 350), (2, "Balcony", 25m, 100),
            (3, "General", 15m, 500), (4, "Workshop", 75m, 30),
            (5, "VIP", 100m, 80), (5, "Standard", 55m, 1500), (5, "Balcony", 40m, 420)
        };
        foreach (var (eid, st, p, tot) in seatData)
        {
            db.EventSeats.Add(new EventSeat { EventID = eid, SeatType = st, Price = p, TotalSeats = tot, AvailableSeats = tot - 10 });
        }
        db.SaveChanges();
    }

    private static void EnsureIsAdminColumn(AppDbContext db)
    {
        try
        {
            if (db.Database.IsSqlServer())
                db.Database.ExecuteSqlRaw("IF COL_LENGTH('Members','IsAdmin') IS NULL ALTER TABLE Members ADD IsAdmin BIT NOT NULL DEFAULT 0");
            else if (db.Database.IsSqlite())
                db.Database.ExecuteSqlRaw("ALTER TABLE Members ADD COLUMN IsAdmin INTEGER NOT NULL DEFAULT 0");
        }
        catch { /* Column may already exist */ }
    }

    private static void EnsureAdminExists(AppDbContext db)
    {
        if (db.Members.Any(m => m.IsAdmin)) return;
        var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        db.Members.Add(new Member { Email = "admin@metro.gov", PasswordHash = adminHash, FirstName = "Admin", LastName = "User", IsAdmin = true });
        db.SaveChanges();
    }
}
