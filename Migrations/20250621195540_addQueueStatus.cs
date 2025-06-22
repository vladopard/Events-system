using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events_system.Migrations
{
    /// <inheritdoc />
    public partial class addQueueStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queues_Tickets_TicketId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Queues");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Queues",
                newName: "TicketTypeId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Queues",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Queues_TicketId",
                table: "Queues",
                newName: "IX_Queues_TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId_Seat",
                table: "Tickets",
                columns: new[] { "EventId", "Seat" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_TicketTypes_TicketTypeId",
                table: "Queues",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queues_TicketTypes_TicketTypeId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId_Seat",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketTypeId",
                table: "Queues",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Queues",
                newName: "Quantity");

            migrationBuilder.RenameIndex(
                name: "IX_Queues_TicketTypeId",
                table: "Queues",
                newName: "IX_Queues_TicketId");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Queues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_Tickets_TicketId",
                table: "Queues",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
