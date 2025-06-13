namespace Events_system.DTOs
{
    public abstract class EventBaseDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Venue { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }

    public class EventCreateDTO : EventBaseDTO { }

    public class EventUpdateDTO : EventBaseDTO { }

    public class EventDTO : EventBaseDTO
    {
        public int Id { get; set; }
    }

    public class EventPatchDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Venue { get; set; }
        public string? City { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
