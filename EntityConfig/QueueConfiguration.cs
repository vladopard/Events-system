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

            builder.Property(q => q.Status)
                   .IsRequired();

            builder.Property(q => q.UserId)
                   .IsRequired();

            builder.HasOne(q => q.User)
                   .WithMany(u => u.Queues)
                   .HasForeignKey(q => q.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.TicketType)
                   .WithMany()
                   .HasForeignKey(q => q.TicketTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Queues");
        }
    }
}
