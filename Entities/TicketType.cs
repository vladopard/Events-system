namespace Events_system.Entities
{
    public class TicketType
    {
        public int Id { get; set; }
        public required string Name { get; set; }  // нпр. "VIP", "Regular"
        public decimal Price { get; set; }

        public int EventId { get; set; }           // ← веза ка Event
        public Event Event { get; set; } = default!;
    }
}
