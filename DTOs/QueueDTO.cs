using Events_system.Entities; // zbog QueueStatus

namespace Events_system.DTOs
{
    public abstract class QueueBaseDTO
    {
        public string UserId { get; set; } = null!;
        public int TicketTypeId { get; set; }
    }

    public class QueueCreateDTO : QueueBaseDTO
    {
    }

    public class QueueUpdateDTO : QueueBaseDTO 
    {
        public QueueStatus Status { get; set; }  // ✅ ok ovde
    }

    public class QueueDTO : QueueBaseDTO
    {
        public int Id { get; set; }
        public QueueStatus Status { get; set; }
        public string TicketTypeName { get; set; } = null!;
    }

    public class QueuePatchDTO
    {
        public string? UserId { get; set; }
        public int? TicketTypeId { get; set; }
        public QueueStatus? Status { get; set; }
    }
}
