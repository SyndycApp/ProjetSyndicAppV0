using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPrestataireNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrestataireNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrestataireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Qualite = table.Column<int>(type: "int", nullable: false),
                    Delai = table.Column<int>(type: "int", nullable: false),
                    Communication = table.Column<int>(type: "int", nullable: false),
                    AuteurSyndicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestataireNotes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrestataireNotes_PrestataireId_AuteurSyndicId",
                table: "PrestataireNotes",
                columns: new[] { "PrestataireId", "AuteurSyndicId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrestataireNotes");
        }
    }
}
