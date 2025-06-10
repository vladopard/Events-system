namespace Events_system.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User User { get; set; } = default!;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
