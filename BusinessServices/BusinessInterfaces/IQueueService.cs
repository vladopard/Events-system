using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface IQueueService
    {
        Task<IEnumerable<QueueDTO>> GetAllAsync();
        Task<QueueDTO> GetByIdAsync(int id);
        Task<IEnumerable<QueueDTO>> GetWaitingAsync();
        Task<IEnumerable<QueueDTO>> GetByUserIdAsync(string userId);
        Task ProcessQueueForTicketTypeAsync(int ticketTypeId);
        Task NotifyAsync(int id);    
        Task DeleteAsync(int id);
    }
}