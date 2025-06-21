namespace Events_system.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public required string Seat { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public int TicketTypeId { get; set; }                  // ← додато
        public TicketType TicketType { get; set; } = default!; // ← додато

        public int? OrderId { get; set; }
        public Order? Order { get; set; }

    }
}
