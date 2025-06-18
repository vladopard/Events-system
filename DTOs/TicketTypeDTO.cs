namespace Events_system.DTOs
{
    public abstract class TicketTypeBaseDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int EventId { get; set; } // ← додато због новог односа

    }

    public class TicketTypeCreateDTO : TicketTypeBaseDTO { }

    public class TicketTypeUpdateDTO : TicketTypeBaseDTO { }

    public class TicketTypeDTO : TicketTypeBaseDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; } = null!; // ако ти треба за фронт

    }

    public class TicketTypePatchDTO
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? EventId { get; set; }

    }
}
