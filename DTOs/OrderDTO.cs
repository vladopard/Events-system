namespace Events_system.DTOs
{
    public abstract class OrderBaseDTO
    {
        public string UserId { get; set; } = null!;
        public List<int> TicketIds { get; set; } = new List<int>();
    }
    public class OrderCreateDTO : OrderBaseDTO { }
    public class OrderUpdateDTO : OrderBaseDTO { }
    //ORDER UPDATE
    public class OrderDTO : OrderBaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketDTO> Tickets { get; set; } = new();

    }

    public class OrderPatchDTO
    {
        public string? UserId { get; set; }
        public List<int>? TicketIds { get; set; }
    }
    //не можеш унапред знати ID тикета — јер тек тражиш први слободан.
    public class OrderRequestDTO
    {
        public string UserId { get; set; } = null!;
        public int TicketTypeId { get; set; }
    }

    public class OrderOrQueueResponseDTO
    {
        public bool IsQueued { get; set; }

        // Ако је наруџбина успела
        public OrderDTO? Order { get; set; }

        // Ако није било доступних карата
        public QueueDTO? Queue { get; set; }
    }
}
