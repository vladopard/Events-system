using Events_system.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Events_system.DbContexts
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // TicketTypes
            modelBuilder.Entity<TicketType>().HasData(
                new TicketType { Id = 1, Name = "Regular", Price = 50 },
                new TicketType { Id = 2, Name = "VIP", Price = 120 },
                new TicketType { Id = 3, Name = "Student", Price = 30 }
            );
            

        }
    }

}
