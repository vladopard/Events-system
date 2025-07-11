﻿using Events_system.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Events_system.DbContexts
{
    public static class BusinessSeed
    {
        public static async Task EnsureAsync(this IServiceProvider sp)
        {
            var db = sp.GetRequiredService<EventDbContext>();
            var um = sp.GetRequiredService<UserManager<User>>();
            var usr = await um.FindByIdAsync("seed-user-id");
            if (usr == null) return;

            if (!await db.Orders.AnyAsync())
            {
                var order = new Order
                {
                    UserId = usr.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(1)
                };
                db.Orders.Add(order);
                await db.SaveChangesAsync();

                var firstTicket = await db.Tickets.FirstAsync();
                firstTicket.OrderId = order.Id;
                await db.SaveChangesAsync();

                var ticketTypeId = firstTicket.TicketTypeId;

                db.Queues.Add(new Queue
                {
                    UserId = usr.Id,
                    TicketTypeId = ticketTypeId,
                    Status = QueueStatus.Waiting
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
