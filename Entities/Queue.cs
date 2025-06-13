namespace Events_system.Entities
{
    public class Queue
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsProcessed { get; set; } = false;
        public required string UserId { get; set; }
        public int TicketId { get; set; }
       

        public User User { get; set; } = default!;
        public Ticket Ticket { get; set; } = default!;


    }
}
