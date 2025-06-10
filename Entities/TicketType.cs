namespace Events_system.Entities
{
    public class TicketType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
