using Events_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events_system.EntityConfig
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Seat)
                .IsRequired()
                .HasMaxLength(50);

            // Ticket → Event (many-to-one)
            builder.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Kad se Event obriše, briši i njegove tikete

            // Ticket → TicketType (many-to-one) ← OVO JE NOVO
            builder.HasOne(t => t.TicketType)
                .WithMany()
                .HasForeignKey(t => t.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Ticket → Order (nullable many-to-one)
            builder.HasOne(t => t.Order)
                .WithMany(o => o.Tickets)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.SetNull); // Ako se Order obriše, Ticket ostaje

            builder
                .HasIndex(t => new { t.EventId, t.Seat })
                .IsUnique();

            builder.ToTable("Tickets");
        }
    }
}
