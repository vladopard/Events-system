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
            .ForMember(d => d.TicketIds,
                       c => c.MapFrom(s => s.Tickets.Select(t => t.Id)))
            // puni listu ticketdto-ova
            .ForMember(d => d.Tickets,
                       c => c.MapFrom(s => s.Tickets));

            CreateMap<OrderCreateDTO, Order>();



            // ======================
            // QUEUE
            // ======================

            CreateMap<Queue, QueueDTO>()
                .ForMember(d => d.TicketTypeName, c => c.MapFrom(s => s.TicketType.Name))
                .ForMember(d => d.Price, c => c.MapFrom(s => s.TicketType.Price))
                .ForMember(d => d.EventName, c => c.MapFrom(s => s.TicketType.Event.Name));


            CreateMap<QueueCreateDTO, Queue>()
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // биће постављен ручно као QueueStatus.Waiting

            CreateMap<QueueUpdateDTO, Queue>();

            CreateMap<QueuePatchDTO, Queue>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
