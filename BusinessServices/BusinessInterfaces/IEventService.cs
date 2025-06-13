using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface IEventService
    {
        Task<EventDTO> CreateAsync(EventCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EventDTO>> GetAllAsync();
        Task<EventDTO> GetByIdAsync(int id);
        Task<bool> PatchAsync(int id, EventPatchDTO dto);
        Task<bool> UpdateAsync(int id, EventUpdateDTO dto);
    }
}