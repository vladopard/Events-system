namespace Events_system.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public required string Venue { get; set; }
        public required string City { get; set; }
        public DateTime StartDate { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
