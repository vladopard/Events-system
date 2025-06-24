using Events_system.DTOs;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateAsync(OrderCreateDTO dto);
        Task<OrderOrQueueResponseDTO> CreateeAsync(OrderRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<OrderDTO>> GetAllAsync();
        Task<OrderDTO> GetByIdAsync(int id);
        Task<IEnumerable<OrderDTO>> GetByUserIdAsync(string userId);
        Task<bool> PatchAsync(int id, OrderPatchDTO dto);
        Task UpdateAsync(int id, OrderUpdateDTO dto);
    }
}