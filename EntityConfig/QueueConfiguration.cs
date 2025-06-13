using Events_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events_system.EntityConfig
{
    public class QueueConfiguration : IEntityTypeConfiguration<Queue>
    {
        public void Configure(EntityTypeBuilder<Queue> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Quantity)
                   .IsRequired();

            builder.Property(q => q.IsProcessed)
                   .IsRequired();

            builder.Property(q => q.UserId)
                   .IsRequired();

            builder.HasOne(q => q.User)
                   .WithMany(u => u.Queues)
                   .HasForeignKey(q => q.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // ако се обрише корисник, обриши и ред из чекања

            builder.HasOne(q => q.Ticket)
                   .WithMany(t => t.Queues)
                   .HasForeignKey(q => q.TicketId)
                   .OnDelete(DeleteBehavior.Cascade); // ако се обрише карта, обриши и ред из чекања

            builder.ToTable("Queues");
        }
    }
}
