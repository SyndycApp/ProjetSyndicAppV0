using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonnelScoreHistoriquR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonnelScoreHistoriques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    Mois = table.Column<int>(type: "int", nullable: false),
                    ScoreBrut = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false),
                    ScoreNormalise = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelScoreHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelScoreHistoriques_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelScoreHistoriques_EmployeId_Annee_Mois",
                table: "PersonnelScoreHistoriques",
                columns: new[] { "EmployeId", "Annee", "Mois" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelScoreHistoriques");
        }
    }
}
