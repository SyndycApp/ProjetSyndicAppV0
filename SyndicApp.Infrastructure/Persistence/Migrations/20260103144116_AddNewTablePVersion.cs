using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTablePVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateArchivage",
                table: "AssembleesGenerales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProcesVerbalVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcesVerbalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroVersion = table.Column<int>(type: "int", nullable: false),
                    Contenu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPdf = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EstOfficielle = table.Column<bool>(type: "bit", nullable: false),
                    DateGeneration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GenereParId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesVerbalVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcesVerbalVersions_ProcesVerbaux_ProcesVerbalId",
                        column: x => x.ProcesVerbalId,
                        principalTable: "ProcesVerbaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcesVerbalVersions_ProcesVerbalId_EstOfficielle",
                table: "ProcesVerbalVersions",
                columns: new[] { "ProcesVerbalId", "EstOfficielle" },
                unique: true,
                filter: "[EstOfficielle] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ProcesVerbalVersions_ProcesVerbalId_NumeroVersion",
                table: "ProcesVerbalVersions",
                columns: new[] { "ProcesVerbalId", "NumeroVersion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "DateArchivage",
                table: "AssembleesGenerales");
        }
    }
}
