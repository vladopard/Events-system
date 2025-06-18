using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Events_system.Repositories;

namespace Events_system.BusinessServices
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ISystemRepository _repo;
        private readonly IMapper _mapper;

        public TicketTypeService(ISystemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        private async Task<TicketType> GetTicketTypeOrThrowAsync(int id)
        {
            return await _repo.GetTicketTypeByIdAsync(id)
                ?? throw new KeyNotFoundException($"TicketType {id} not found.");
        }

        public async Task<IEnumerable<TicketTypeDTO>> GetAllAsync()
        {
            var types = await _repo.GetAllTicketTypesAsync();
            return _mapper.Map<IEnumerable<TicketTypeDTO>>(types);
        }

        public async Task<TicketTypeDTO> GetByIdAsync(int id)
        {
            var type = await GetTicketTypeOrThrowAsync(id);
            return _mapper.Map<TicketTypeDTO>(type);
        }
        public async Task<IEnumerable<TicketTypeDTO>> GetByEventIdAsync(int eventId)
        {
            var types = await _repo.GetTicketTypesByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<TicketTypeDTO>>(types);
        }
        public async Task<TicketTypeDTO> CreateAsync(TicketTypeCreateDTO dto)
        {
            var entity = _mapper.Map<TicketType>(dto);
            await _repo.AddTicketTypeAsync(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<TicketTypeDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, TicketTypeUpdateDTO dto)
        {
            var entity = await GetTicketTypeOrThrowAsync(id);
            _mapper.Map(dto, entity);
            _repo.UpdateTicketType(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchAsync(int id, TicketTypePatchDTO dto)
        {
            var entity = await GetTicketTypeOrThrowAsync(id);
            _mapper.Map(dto, entity);
            _repo.UpdateTicketType(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetTicketTypeOrThrowAsync(id);
            _repo.DeleteTicketType(entity);
            return await _repo.SaveChangesAsync();
        }

    }
}
