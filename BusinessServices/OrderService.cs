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

        //public async Task<OrderDTO> CreateAsync(OrderCreateDTO dto)
        //{
        //    var order = _mapper.Map<Order>(dto);
        //    await _repo.AddOrderAsync(order);

        //    await _repo.SaveChangesAsync();

        //    var newOrder = await _repo.GetOrderByIdAsync(order.Id);

        //    return _mapper.Map<OrderDTO>(newOrder);
        //}

        public async Task<OrderOrQueueResponseDTO> CreateeAsync(OrderRequestDTO dto)
        {
            var availableTickets = await _repo.GetTicketsByTicketTypeIdAsync(dto.TicketTypeId);
            var ticket = availableTickets.FirstOrDefault(t => t.OrderId == null);

            if (ticket == null)
            {
                // Нема слободних → додај у ред чекања
                var queue = new Queue
                {
                    UserId = dto.UserId,
                    TicketTypeId = dto.TicketTypeId,
                    Status = QueueStatus.Waiting
                };

                await _repo.AddQueueAsync(queue);
                await _repo.SaveChangesAsync();

                // Поново учитати queue са Ticket-ом и User-ом
                var createdQueue = await _repo.GetQueueByIdAsync(queue.Id);

                return new OrderOrQueueResponseDTO
                {
                    IsQueued = true,
                    Queue = _mapper.Map<QueueDTO>(createdQueue)
                };
            }

            // Постоји слободна карта → креирај поруџбину
            var order = new Order
            {
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                Tickets = new List<Ticket> { ticket }
            };

            ticket.Order = order;

            await _repo.AddOrderAsync(order);
            await _repo.SaveChangesAsync();

            // Поново учитати order са Tickets
            var createdOrder = await _repo.GetOrderByIdAsync(order.Id);

            return new OrderOrQueueResponseDTO
            {
                IsQueued = false,
                Order = _mapper.Map<OrderDTO>(createdOrder)
            };
        }

        public async Task<OrderDTO> CreateAsync(OrderCreateDTO dto)
        {

            var order = _mapper.Map<Order>(dto);

            foreach (var ticketId in dto.TicketIds)
            {
                var ticket = await _repo.GetTicketByIdAsync(ticketId);
                if (ticket == null) throw new KeyNotFoundException($"Ticket {ticketId} not found.");

                // Proveri da nije već dodeljen
                if (ticket.OrderId != null)
                    throw new InvalidOperationException($"Ticket {ticketId} is already assigned to an order.");

                ticket.Order = order; // EF automatski povezuje
                order.Tickets.Add(ticket);
            }

            await _repo.AddOrderAsync(order);
            await _repo.SaveChangesAsync();

            var newOrder = await _repo.GetOrderByIdAsync(order.Id); // .Include(o => o.Tickets)
            return _mapper.Map<OrderDTO>(newOrder);
        }


        public async Task UpdateAsync(int id, OrderUpdateDTO dto)
        {
            var order = await GetOrderOrThrowAsync(id);

            // 1. Обриши све постојеће везе
            var existingTickets = await _repo.GetTicketsByOrderIdAsync(order.Id);
            foreach (var ticket in existingTickets)
                ticket.OrderId = null;

            // 2. Апдејтуј основне податке
            order.UserId = dto.UserId;

            // 3. Вежи нове тикете
            foreach (var ticketId in dto.TicketIds)
            {
                var ticket = await _repo.GetTicketByIdAsync(ticketId);
                if (ticket == null)
                    throw new KeyNotFoundException($"Ticket {ticketId} not found.");

                if (ticket.OrderId != null && ticket.OrderId != order.Id)
                    throw new InvalidOperationException($"Ticket {ticketId} is already assigned to another order.");

                ticket.OrderId = order.Id;
            }

            await _repo.SaveChangesAsync();
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
