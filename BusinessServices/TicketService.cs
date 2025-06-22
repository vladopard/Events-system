using AutoMapper;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Events_system.Repositories;

namespace Events_system.BusinessServices
{
    public class TicketService : ITicketService
    {
        private readonly ISystemRepository _repo;
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;

        public TicketService(ISystemRepository repo, IMapper mapper, IQueueService queueService)
        {
            _repo = repo;
            _mapper = mapper;
            _queueService = queueService;
        }

        public async Task<IEnumerable<TicketDTO>> GetAllAsync()
        {
            var tickets = await _repo.GetAllTicketsAsync();
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public async Task<TicketDTO> GetByIdAsync(int id)
        {
            var ticket = await GetTicketOrThrowAsync(id);
            return _mapper.Map<TicketDTO>(ticket);
        }

        //GetTicketsByEventId(int eventId)

        public async Task<TicketDTO> CreateAsync(TicketCreateDTO dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);
            await _repo.AddTicketAsync(ticket);

            await _repo.SaveChangesAsync();
            //PROVERI DAL RADI
            await _queueService.ProcessQueueForTicketTypeAsync(ticket.TicketTypeId);

            var newTicket = await _repo.GetTicketByIdAsync(ticket.Id);

            return _mapper.Map<TicketDTO>(newTicket);
        }
        public async Task<bool> UpdateAsync(int id, TicketUpdateDTO dto)
        {
            var ticket = await GetTicketOrThrowAsync(id);
            _mapper.Map(dto, ticket);
            _repo.UpdateTicket(ticket);
            return await _repo.SaveChangesAsync();
        }
        public async Task<bool> PatchAsync(int id, TicketPatchDTO dto)
        {
            var ticket = await GetTicketOrThrowAsync(id);
            _mapper.Map(dto, ticket);
            _repo.UpdateTicket(ticket);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await GetTicketOrThrowAsync(id);
            _repo.DeleteTicket(ticket);
            return await _repo.SaveChangesAsync();
        }

        private async Task<Ticket> GetTicketOrThrowAsync(int id)
        {
            return await _repo.GetTicketByIdAsync(id)
                ?? throw new KeyNotFoundException($"Ticket {id} not found.");
        }
    }
}
