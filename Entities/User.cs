using Microsoft.AspNetCore.Identity;

namespace Events_system.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateJoined { get; set; } =
            DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Queue> Queues { get; set; } = new List<Queue>();

        /* iz Identrity user dobijas ID, username,email, passwordhash, itd...  */
    }
}

