using Events_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events_system.EntityConfig
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Venue)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(500);

            builder.Property(e => e.StartDate)
                .IsRequired();

            // (опционо) да датум не буде у прошлости — само преко валидације, не преко Fluent API
        }
    }
}
