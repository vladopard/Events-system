namespace Events_system.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
    }

    public record LoginDTO(string Email, string Password);
    public record TokenResponseDTO
    {
        public string Token { get; init; } = default!;
        public DateTime ExpiresAt { get; init; }
    }
}
