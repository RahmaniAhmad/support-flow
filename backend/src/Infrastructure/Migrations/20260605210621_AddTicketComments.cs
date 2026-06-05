using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "TicketComments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TicketComments",
                newName: "AuthorUserId");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "TicketComments",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "TicketComments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Tickets_TicketId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "TicketComments");

            migrationBuilder.RenameColumn(
                name: "AuthorUserId",
                table: "TicketComments",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TicketComments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
