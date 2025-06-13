namespace Events_system.DTOs
{
    public abstract class QueueBaseDTO
    {
        public int Quantity { get; set; }
        public bool IsProcessed { get; set; }
        public string UserId { get; set; } = null!;
        public int TicketId { get; set; }
    }

    public class QueueCreateDTO : QueueBaseDTO { }

    public class QueueUpdateDTO : QueueBaseDTO { }

    public class QueueDTO : QueueBaseDTO
    {
        public int Id { get; set; }
        public string TicketSeat { get; set; } = null!;
    }

    public class QueuePatchDTO
    {
        public int? Quantity { get; set; }
        public bool? IsProcessed { get; set; }
        public string? UserId { get; set; }
        public int? TicketId { get; set; }
    }
}
