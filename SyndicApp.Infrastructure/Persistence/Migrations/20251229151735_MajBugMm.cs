using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MajBugMm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionValidation_PlanningMissions_PlanningMissionId",
                table: "MissionValidation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MissionValidation",
                table: "MissionValidation");

            migrationBuilder.RenameTable(
                name: "MissionValidation",
                newName: "MissionValidations");

            migrationBuilder.RenameIndex(
                name: "IX_MissionValidation_PlanningMissionId",
                table: "MissionValidations",
                newName: "IX_MissionValidations_PlanningMissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MissionValidations",
                table: "MissionValidations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionValidations_PlanningMissions_PlanningMissionId",
                table: "MissionValidations",
                column: "PlanningMissionId",
                principalTable: "PlanningMissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionValidations_PlanningMissions_PlanningMissionId",
                table: "MissionValidations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MissionValidations",
                table: "MissionValidations");

            migrationBuilder.RenameTable(
                name: "MissionValidations",
                newName: "MissionValidation");

            migrationBuilder.RenameIndex(
                name: "IX_MissionValidations_PlanningMissionId",
                table: "MissionValidation",
                newName: "IX_MissionValidation_PlanningMissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MissionValidation",
                table: "MissionValidation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionValidation_PlanningMissions_PlanningMissionId",
                table: "MissionValidation",
                column: "PlanningMissionId",
                principalTable: "PlanningMissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
