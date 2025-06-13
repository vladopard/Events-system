using Events_system.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Events_system.DbContexts
{
    public class EventDbContext : IdentityDbContext<User>
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedData.Seed(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);

        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Queue> Queues { get; set; }
    }
}
