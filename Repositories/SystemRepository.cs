using System.Threading.Tasks;
using Events_system.DbContexts;
using Events_system.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events_system.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly EventDbContext _context;

        public SystemRepository(EventDbContext context)
        {
            _context = context;
        }

        // EVENTS
        public async Task<IEnumerable<Event>> GetAllEventsAsync() =>
            await _context.Events
                .AsNoTracking()
                .Include(e => e.Tickets)
                .Include(e => e.TicketTypes)
                .ToListAsync();

        public async Task<Event?> GetEventByIdAsync(int id) =>
            await _context.Events
                .Include(e => e.Tickets)
                .Include(e => e.TicketTypes)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task AddEventAsync(Event evt) => await _context.Events.AddAsync(evt);
        public void UpdateEvent(Event evt) => _context.Events.Update(evt);
        public void DeleteEvent(Event evt) => _context.Events.Remove(evt);

        // TICKETS
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync() =>
            await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.TicketType)
                .ToListAsync();

        public async Task<Ticket?> GetTicketByIdAsync(int id) =>
            await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(t => t.Id == id);
        public async Task<IEnumerable<Ticket>> GetTicketsByTicketTypeIdAsync(int ticketTypeId)
        {
            return await _context.Tickets
                .Where(t => t.TicketTypeId == ticketTypeId && t.OrderId == null)
                .Include(t => t.TicketType)
                .Include(t => t.Event)
                .ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByOrderIdAsync(int orderId) =>
            await _context.Tickets
                .Where(t => t.OrderId == orderId)
                .ToListAsync();

        public async Task AddTicketAsync(Ticket ticket) => await _context.Tickets.AddAsync(ticket);
        public void UpdateTicket(Ticket ticket) => _context.Tickets.Update(ticket);
        public void DeleteTicket(Ticket ticket) => _context.Tickets.Remove(ticket);

        // ORDERS
        public async Task<IEnumerable<Order>> GetAllOrdersAsync() =>
            await _context.Orders
                .Include(o => o.Tickets)
                .Include(o => o.User)
                .ToListAsync();

        public async Task<Order?> GetOrderByIdAsync(int id) =>
            await _context.Orders
                .Include(o => o.Tickets)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddOrderAsync(Order order) => await _context.Orders.AddAsync(order);
        public void UpdateOrder(Order order) => _context.Orders.Update(order);
        public void DeleteOrder(Order order) => _context.Orders.Remove(order);

        // QUEUES
        public async Task<IEnumerable<Queue>> GetAllQueuesAsync() =>
            await _context.Queues
                .Include(q => q.TicketType)
                .Include(q => q.User)
                .ToListAsync();

        public async Task<Queue?> GetQueueByIdAsync(int id) =>
            await _context.Queues
                .Include(q => q.TicketType)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == id);

        public async Task AddQueueAsync(Queue queue) => await _context.Queues.AddAsync(queue);
        public void UpdateQueue(Queue queue) => _context.Queues.Update(queue);
        public void DeleteQueue(Queue queue) => _context.Queues.Remove(queue);

        // TICKET TYPES
        public async Task<IEnumerable<TicketType>> GetAllTicketTypesAsync() =>
            await _context.TicketTypes
                .Include(tt => tt.Event)
                .ToListAsync();

        public async Task<TicketType?> GetTicketTypeByIdAsync(int id) =>
            await _context.TicketTypes
                .Include(tt => tt.Event)
                .FirstOrDefaultAsync(tt => tt.Id == id);

        public async Task<IEnumerable<TicketType>> GetTicketTypesByEventIdAsync(int eventId) =>
            await _context.TicketTypes
                .Where(tt => tt.EventId == eventId)
                .Include(tt => tt.Event)
                .ToListAsync();

        public async Task AddTicketTypeAsync(TicketType ticketType) => await _context.TicketTypes.AddAsync(ticketType);
        public void UpdateTicketType(TicketType ticketType) => _context.TicketTypes.Update(ticketType);
        public void DeleteTicketType(TicketType ticketType) => _context.TicketTypes.Remove(ticketType);

        // SAVE CHANGES
        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
