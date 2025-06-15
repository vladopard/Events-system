using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface IQueueService
    {
        Task<QueueDTO> CreateAsync(QueueCreateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<QueueDTO>> GetAllAsync();
        Task<QueueDTO> GetByIdAsync(int id);
        Task PatchAsync(int id, QueuePatchDTO dto);
        Task UpdateAsync(int id, QueueUpdateDTO dto);
    }
}