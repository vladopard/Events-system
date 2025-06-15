using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface ITicketService
    {
        Task<TicketDTO> CreateAsync(TicketCreateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TicketDTO>> GetAllAsync();
        Task<bool> UpdateAsync(int id, TicketCreateDTO dto);
        Task<bool> PatchAsync(int id, TicketPatchDTO dto);

        Task<TicketDTO> GetByIdAsync(int id);
    }
}