namespace Events_system.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public required string Seat { get; set; }
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public int? OrderId { get; set; }

        public Event Event { get; set; } = default!;
        public TicketType TicketType { get; set; } = default!;
        public Order? Order { get; set; }
        public ICollection<Queue> Queues { get; set; } = new List<Queue>();

    }
}
