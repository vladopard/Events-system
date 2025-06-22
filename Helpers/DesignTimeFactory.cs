using Events_system.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Events_system.Helpers
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        public EventDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<EventDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=eventsdb;Username=postgres;Password=password123")
                .Options;
            return new EventDbContext(options);
        }
    }
}