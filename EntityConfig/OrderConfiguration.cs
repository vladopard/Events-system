using Events_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events_system.EntityConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId)
                   .IsRequired();

            builder.Property(o => o.CreatedAt)
                   .IsRequired();

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // Не бриши поруџбине ако се обрише User

            // 🔒 Check constraint: CreatedAt must not be in the future
            //builder.ToTable("Orders", table =>
            //{
            //    table.HasCheckConstraint(
            //        "CK_Order_CreatedAt_Past",
            //        "\"CreatedAt\" <= CURRENT_TIMESTAMP"
            //    );
            //});
        }
    }
}
