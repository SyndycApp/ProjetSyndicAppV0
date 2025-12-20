using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCallModuleV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Calls_CallerId",
                table: "Calls",
                column: "CallerId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_ReceiverId",
                table: "Calls",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_StartedAt",
                table: "Calls",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_Status",
                table: "Calls",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Calls_CallerId",
                table: "Calls");

            migrationBuilder.DropIndex(
                name: "IX_Calls_ReceiverId",
                table: "Calls");

            migrationBuilder.DropIndex(
                name: "IX_Calls_StartedAt",
                table: "Calls");

            migrationBuilder.DropIndex(
                name: "IX_Calls_Status",
                table: "Calls");
        }
    }
}
