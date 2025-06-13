namespace Events_system.DTOs
{
    public abstract class OrderBaseDTO
    {
        public string UserId { get; set; } = null!;
    }
    public class OrderCreateDTO : OrderBaseDTO { }

    //ORDER UPDATE
    public class OrderDTO : OrderBaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> TicketIds { get; set; } = new();
    }
}
