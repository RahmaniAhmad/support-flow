using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketCommentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Tickets_TicketId1",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_TicketId1",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "TicketId1",
                table: "TicketComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "KnowledgeArticles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_AuthorUserId",
                table: "TicketComments",
                column: "AuthorUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketComments_AuthorUserId",
                table: "TicketComments");

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId1",
                table: "TicketComments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "KnowledgeArticles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId1",
                table: "TicketComments",
                column: "TicketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Tickets_TicketId1",
                table: "TicketComments",
                column: "TicketId1",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
