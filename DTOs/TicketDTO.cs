namespace Events_system.DTOs
{
    public abstract class TicketBaseDTO
    {
        public string Seat { get; set; } = null!;
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public int? OrderId { get; set; }
    }

    public class TicketCreateDTO : TicketBaseDTO { }

    public class TicketUpdateDTO : TicketBaseDTO { }

    public class TicketDTO : TicketBaseDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; } = null!;
        public string TicketTypeName { get; set; } = null!;
        public decimal TicketPrice { get; set; }
    }

    public class TicketPatchDTO
    {
        public string? Seat { get; set; }
        public int? EventId { get; set; }
        public int? TicketTypeId { get; set; }
        public int? OrderId { get; set; }
    }
}
