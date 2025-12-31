using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModuleAssembleesNiv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SeuilMajorite",
                table: "Resolutions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeMajorite",
                table: "Resolutions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PresenceAss",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssembleeGeneraleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Tantiemes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DatePresence = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresenceAss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresenceAss_AssembleesGenerales_AssembleeGeneraleId",
                        column: x => x.AssembleeGeneraleId,
                        principalTable: "AssembleesGenerales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssembleeGeneraleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonneurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MandataireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procurations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresenceAss_AssembleeGeneraleId_UserId",
                table: "PresenceAss",
                columns: new[] { "AssembleeGeneraleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procurations_AssembleeGeneraleId_DonneurId",
                table: "Procurations",
                columns: new[] { "AssembleeGeneraleId", "DonneurId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresenceAss");

            migrationBuilder.DropTable(
                name: "Procurations");

            migrationBuilder.DropColumn(
                name: "SeuilMajorite",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "TypeMajorite",
                table: "Resolutions");
        }
    }
}
