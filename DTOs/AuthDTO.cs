namespace Events_system.DTOs
{
    public class AuthRegisterDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
    }

    public class AuthLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthResultDTO
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
