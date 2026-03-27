namespace EventManagementSystem.Models;

public class Venue
{
    public int VenueID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Description { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}
