using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyTicketCounterAndTicketNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TicketNumber",
                table: "Tickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CompanyTicketCounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastTicketNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTicketCounters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompanyId_TicketNumber",
                table: "Tickets",
                columns: new[] { "CompanyId", "TicketNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTicketCounters_CompanyId",
                table: "CompanyTicketCounters",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTicketCounters");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CompanyId_TicketNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "Tickets");
        }
    }
}
