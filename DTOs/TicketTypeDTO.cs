namespace Events_system.DTOs
{
    public abstract class TicketTypeBaseDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }

    public class TicketTypeCreateDTO : TicketTypeBaseDTO { }

    public class TicketTypeUpdateDTO : TicketTypeBaseDTO { }

    public class TicketTypeDTO : TicketTypeBaseDTO
    {
        public int Id { get; set; }
    }

    public class TicketTypePatchDTO
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
