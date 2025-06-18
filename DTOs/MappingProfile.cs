using AutoMapper;
using Events_system.DTOs;
using Events_system.Entities;

namespace Events_system.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ======================
            // EVENT
            // ======================

            CreateMap<Event, EventDTO>();

            CreateMap<EventCreateDTO, Event>()
                .ForMember(dest => dest.TicketTypes, opt => opt.MapFrom(src => src.TicketTypes));

            CreateMap<EventUpdateDTO, Event>();

            CreateMap<EventDTO, EventPatchDTO>();

            CreateMap<EventPatchDTO, Event>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ======================
            // TICKET
            // ======================

            CreateMap<Ticket, TicketDTO>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.Name))
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketType.Price));

            CreateMap<TicketCreateDTO, Ticket>();
            CreateMap<TicketUpdateDTO, Ticket>();
            CreateMap<TicketDTO, TicketPatchDTO>();

            CreateMap<TicketPatchDTO, Ticket>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ======================
            // TICKET TYPE
            // ======================

            CreateMap<TicketType, TicketTypeDTO>();
            CreateMap<TicketTypeCreateDTO, TicketType>();
            CreateMap<TicketTypeUpdateDTO, TicketType>();
            CreateMap<TicketTypeDTO, TicketTypePatchDTO>();

            CreateMap<TicketTypePatchDTO, TicketType>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ======================
            // ORDER
            // ======================

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.TicketIds, opt => opt.MapFrom(src => src.Tickets.Select(t => t.Id)));

            CreateMap<OrderCreateDTO, Order>();

            // ======================
            // QUEUE
            // ======================

            CreateMap<Queue, QueueDTO>()
                .ForMember(dest => dest.TicketSeat, opt => opt.MapFrom(src => src.Ticket.Seat));

            CreateMap<QueueCreateDTO, Queue>();
            CreateMap<QueueUpdateDTO, Queue>();

            CreateMap<QueuePatchDTO, Queue>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
