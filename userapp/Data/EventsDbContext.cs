using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using userapp.Models;

public class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<Booking> Bookings { get; set; }
}
