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
            var queue = await GetQueueOrThrowAsync(id);
            return _mapper.Map<QueueDTO>(queue);
        }

        public async Task<QueueDTO> CreateAsync(QueueCreateDTO dto)
        {
            var queue = _mapper.Map<Queue>(dto);

            await _repo.AddQueueAsync(queue);
            await _repo.SaveChangesAsync();

            var newQueue = await _repo.GetQueueByIdAsync(queue.Id);
            return _mapper.Map<QueueDTO>(newQueue);
        }

        public async Task UpdateAsync(int id, QueueUpdateDTO dto)
        {
            var queue = await GetQueueOrThrowAsync(id);
            _mapper.Map(dto, queue);
            _repo.UpdateQueue(queue);
            await _repo.SaveChangesAsync();
        }

        public async Task PatchAsync(int id, QueuePatchDTO dto)
        {
            var queue = await GetQueueOrThrowAsync(id);
            _mapper.Map(dto, queue);
            _repo.UpdateQueue(queue);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var queue = await GetQueueOrThrowAsync(id);
            _repo.DeleteQueue(queue);
            await _repo.SaveChangesAsync();
        }

        private async Task<Queue> GetQueueOrThrowAsync(int id)
        {
            return await _repo.GetQueueByIdAsync(id)
                ?? throw new KeyNotFoundException($"Queue {id} not found.");
        }
    }
}
