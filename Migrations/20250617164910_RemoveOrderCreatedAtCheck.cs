using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events_system.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderCreatedAtCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_CreatedAt_Past",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_CreatedAt_Past",
                table: "Orders",
                sql: "\"CreatedAt\" <= CURRENT_TIMESTAMP");
        }
    }
}
