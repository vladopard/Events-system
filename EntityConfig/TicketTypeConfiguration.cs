using Events_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events_system.EntityConfig
{
    public class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
    {
        public void Configure(EntityTypeBuilder<TicketType> builder)
        {
            builder.HasKey(tt => tt.Id);

            builder.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tt => tt.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(tt => tt.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(tt => tt.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("TicketTypes", table =>
            {
                table.HasCheckConstraint(
                    "CK_TicketType_Price_Positive",
                    "\"Price\" > 0"
                );
            });
        }
    }
}
