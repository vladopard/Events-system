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
    }

    public class OrderPatchDTO
    {
        public string? UserId { get; set; }
        public List<int>? TicketIds { get; set; }
    }
}
