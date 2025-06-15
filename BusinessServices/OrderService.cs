using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Events_system.Repositories;

namespace Events_system.BusinessServices
{
    public class OrderService : IOrderService
    {
        private readonly ISystemRepository _repo;
        private readonly IMapper _mapper;

        public OrderService(ISystemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _repo.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            var order = await GetOrderOrThrowAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateAsync(OrderCreateDTO dto)
        {
            var order = _mapper.Map<Order>(dto);
            await _repo.AddOrderAsync(order);

            await _repo.SaveChangesAsync();

            var newOrder = await _repo.GetOrderByIdAsync(order.Id);

            return _mapper.Map<OrderDTO>(newOrder);
        }

        public async Task<bool> UpdateAsync(int id, OrderUpdateDTO dto)
        {
            var order = await GetOrderOrThrowAsync(id);
            _mapper.Map(dto, order);
            _repo.UpdateOrder(order);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchAsync(int id, OrderPatchDTO dto)
        {
            var order = await GetOrderOrThrowAsync(id);
            _mapper.Map(dto, order);
            _repo.UpdateOrder(order);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await GetOrderOrThrowAsync(id);
            _repo.DeleteOrder(order);
            return await _repo.SaveChangesAsync();
        }

        private async Task<Order> GetOrderOrThrowAsync(int id)
        {
            return await _repo.GetOrderByIdAsync(id)
                ?? throw new KeyNotFoundException($"Order {id} not found.");
        }
    }
}
