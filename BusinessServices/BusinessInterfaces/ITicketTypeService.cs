using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface ITicketTypeService
    {
        Task<TicketTypeDTO> CreateAsync(TicketTypeCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TicketTypeDTO>> GetAllAsync();
        Task<TicketTypeDTO> GetByIdAsync(int id);
        Task<IEnumerable<TicketTypeDTO>> GetByEventIdAsync(int eventId);
        Task<bool> PatchAsync(int id, TicketTypePatchDTO dto);
        Task<bool> UpdateAsync(int id, TicketTypeUpdateDTO dto);
    }
}