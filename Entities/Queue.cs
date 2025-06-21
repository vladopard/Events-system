namespace Events_system.Entities
{
    public enum QueueStatus
    {
        Waiting = 0,
        Notified = 1,
        Fulfilled = 2,
        Cancelled = 3
    }

    public class Queue
    {
        public int Id { get; set; }

        public required string UserId { get; set; }
        public User User { get; set; } = default!;

        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; } = default!;

        public QueueStatus Status { get; set; } = QueueStatus.Waiting;
    }

}
