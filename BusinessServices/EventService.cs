using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Events_system.Helpers;
using Events_system.Repositories;

namespace Events_system.BusinessServices
{
    public class EventService : IEventService
    {
        //NISU VRACENE SVE KARTE, MOZDA KASNIJE ZATREBA
        private readonly ISystemRepository _repo;
        private readonly IMapper _mapper;

        public EventService(ISystemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDTO>> GetAllAsync()
        {
            var events = await _repo.GetAllEventsAsync();
            return _mapper.Map<IEnumerable<EventDTO>>(events);
        }

        public async Task<PagedList<EventDTO>> GetAllAsync(EventQueryParameters p)
        {
            // узми страну из репо-ја
            var page = await _repo.GetEventsPagedAsync(p);

            // мапирај само ставке, метаподаци остају исти
            var dtoItems = _mapper.Map<List<EventDTO>>(page);

            return new PagedList<EventDTO>(
                dtoItems,
                page.MetaData.TotalCount,
                page.MetaData.CurrentPage,
                page.MetaData.PageSize);
        }

        public async Task<EventDTO> GetByIdAsync(int id)
        {
            var evt = await GetEventOrThrowAsync(id);
            return _mapper.Map<EventDTO>(evt);
        }

        public async Task<EventDTO> CreateAsync(EventCreateDTO dto)
        {
            var evt = _mapper.Map<Event>(dto);
            await _repo.AddEventAsync(evt);
            await _repo.SaveChangesAsync();
            //AKO NE BUDE RADILO DA ZNAS TREBA ONO U REPOU DA SE PROMENI
            return _mapper.Map<EventDTO>(evt);
        }

        public async Task<bool> UpdateAsync(int id, EventUpdateDTO dto)
        {
            var evt = await GetEventOrThrowAsync(id);

            _mapper.Map(dto, evt);
            _repo.UpdateEvent(evt);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchAsync(int id, EventPatchDTO dto)
        {
            var evt = await GetEventOrThrowAsync(id);

            _mapper.Map(dto, evt);
            _repo.UpdateEvent(evt);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var evt = await GetEventOrThrowAsync(id);

            _repo.DeleteEvent(evt);
            return await _repo.SaveChangesAsync();
        }

        private async Task<Event> GetEventOrThrowAsync(int id)
        {
            return await _repo.GetEventByIdAsync(id)
                ?? throw new KeyNotFoundException($"Event {id} not found.");
        }

    }
}
