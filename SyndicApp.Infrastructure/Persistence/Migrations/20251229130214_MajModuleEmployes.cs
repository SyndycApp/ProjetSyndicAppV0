using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MajModuleEmployes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DocumentsRH",
                newName: "EmployeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsGeoValidated",
                table: "Presences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Presences",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Presences",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanningMissionId",
                table: "Presences",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeAffectationResidences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleSurSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeAffectationResidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeAffectationResidences_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeAffectationResidences_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanningMissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mission = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    HeureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Planifiee"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningMissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningMissions_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanningMissions_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presences_PlanningMissionId",
                table: "Presences",
                column: "PlanningMissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsRH_EmployeId",
                table: "DocumentsRH",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeAffectationResidences_EmployeId_ResidenceId",
                table: "EmployeAffectationResidences",
                columns: new[] { "EmployeId", "ResidenceId" },
                unique: true,
                filter: "[DateFin] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeAffectationResidences_EmployeId_ResidenceId_DateDebut",
                table: "EmployeAffectationResidences",
                columns: new[] { "EmployeId", "ResidenceId", "DateDebut" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeAffectationResidences_ResidenceId",
                table: "EmployeAffectationResidences",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningMissions_EmployeId_Date",
                table: "PlanningMissions",
                columns: new[] { "EmployeId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningMissions_ResidenceId",
                table: "PlanningMissions",
                column: "ResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentsRH_Employes_EmployeId",
                table: "DocumentsRH",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_PlanningMissions_PlanningMissionId",
                table: "Presences",
                column: "PlanningMissionId",
                principalTable: "PlanningMissions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentsRH_Employes_EmployeId",
                table: "DocumentsRH");

            migrationBuilder.DropForeignKey(
                name: "FK_Presences_PlanningMissions_PlanningMissionId",
                table: "Presences");

            migrationBuilder.DropTable(
                name: "EmployeAffectationResidences");

            migrationBuilder.DropTable(
                name: "PlanningMissions");

            migrationBuilder.DropIndex(
                name: "IX_Presences_PlanningMissionId",
                table: "Presences");

            migrationBuilder.DropIndex(
                name: "IX_DocumentsRH_EmployeId",
                table: "DocumentsRH");

            migrationBuilder.DropColumn(
                name: "IsGeoValidated",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "PlanningMissionId",
                table: "Presences");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "DocumentsRH",
                newName: "UserId");
        }
    }
}
