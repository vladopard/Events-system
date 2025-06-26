using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Events_system.Repositories;

namespace Events_system.BusinessServices
{
    public class QueueService : IQueueService
    {
        private readonly ISystemRepository _repo;
        private readonly IMapper _mapper;

        public QueueService(ISystemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QueueDTO>> GetAllAsync()
        {
            var queues = await _repo.GetAllQueuesAsync();
            return _mapper.Map<IEnumerable<QueueDTO>>(queues);
        }

        public async Task<QueueDTO> GetByIdAsync(int id)
        {
            var q = await GetOrThrow(id);
            return _mapper.Map<QueueDTO>(q);
        }

        public async Task<IEnumerable<QueueDTO>> GetWaitingAsync()
        {
            var queues = await _repo.GetAllQueuesAsync();
            var waiting = queues.Where(q => q.Status == QueueStatus.Waiting);
            return _mapper.Map<IEnumerable<QueueDTO>>(waiting);
        }

        public async Task<IEnumerable<QueueDTO>> GetByUserIdAsync(string userId)
        {
            var queues = await _repo.GetQueuesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<QueueDTO>>(queues);
        }


        public async Task NotifyAsync(int id)
        {
            var q = await GetOrThrow(id);
            if (q.Status != QueueStatus.Waiting)
                throw new InvalidOperationException("Only waiting queues can be notified.");

            q.Status = QueueStatus.Notified;
            _repo.UpdateQueue(q);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var q = await GetOrThrow(id);
            _repo.DeleteQueue(q);
            await _repo.SaveChangesAsync();
        }

        // ────────────────────────────────────────────────
        private async Task<Queue> GetOrThrow(int id) =>
            await _repo.GetQueueByIdAsync(id)
            ?? throw new KeyNotFoundException($"Queue {id} not found.");

        public async Task ProcessQueueForTicketTypeAsync(int ticketTypeId)
        {
            // 1. има ли слободних карата?
            var freeTicket = (await _repo.GetTicketsByTicketTypeIdAsync(ticketTypeId))
                             .FirstOrDefault();
            if (freeTicket == null) return;

            // 2. има ли неког ко чека?
            var queueItem = (await _repo.GetWaitingAsync())
                            .Where(q => q.TicketTypeId == ticketTypeId)
                            .OrderBy(q => q.Id)          // FIFO
                            .FirstOrDefault();
            if (queueItem == null) return;

            // 3. креирај поруџбину
            var order = new Order
            {
                UserId = queueItem.UserId,
                CreatedAt = DateTime.UtcNow,
                Tickets = new List<Ticket> { freeTicket }
            };
            freeTicket.Order = order;
            await _repo.AddOrderAsync(order);

            // 4. уклони/заврши queue
            queueItem.Status = QueueStatus.Fulfilled;
            _repo.UpdateQueue(queueItem);

            await _repo.SaveChangesAsync();
        }


    }
}
