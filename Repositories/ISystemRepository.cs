using System.Threading.Tasks;
using Events_system.Entities;

namespace Events_system.Repositories
{
    public interface ISystemRepository
    {
        Task AddEventAsync(Event evt);
        Task AddOrderAsync(Order order);
        Task AddQueueAsync(Queue queue);
        Task AddTicketAsync(Ticket ticket);
        Task AddTicketTypeAsync(TicketType ticketType);
        Task<IEnumerable<Queue>> GetWaitingAsync();
        void DeleteEvent(Event evt);
        void DeleteOrder(Order order);
        void DeleteQueue(Queue queue);
        void DeleteTicket(Ticket ticket);
        void DeleteTicketType(TicketType ticketType);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Queue>> GetAllQueuesAsync();
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<IEnumerable<TicketType>> GetAllTicketTypesAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Queue?> GetQueueByIdAsync(int id);
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<TicketType?> GetTicketTypeByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetTicketsByOrderIdAsync(int orderId);
        Task<IEnumerable<Ticket>> GetTicketsByTicketTypeIdAsync(int ticketTypeId);
        Task<IEnumerable<TicketType>> GetTicketTypesByEventIdAsync(int eventId);
        Task<bool> SaveChangesAsync();
        void UpdateEvent(Event evt);
        void UpdateOrder(Order order);
        void UpdateQueue(Queue queue);
        void UpdateTicket(Ticket ticket);
        void UpdateTicketType(TicketType ticketType);
    }
}