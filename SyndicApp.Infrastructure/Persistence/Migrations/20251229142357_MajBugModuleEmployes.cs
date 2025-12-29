using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MajBugModuleEmployes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeAffectationResidences_Employes_EmployeId",
                table: "EmployeAffectationResidences");

            migrationBuilder.DropIndex(
                name: "IX_EmployeAffectationResidences_EmployeId_ResidenceId_DateDebut",
                table: "EmployeAffectationResidences");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "EmployeAffectationResidences",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeAffectationResidences_EmployeId_ResidenceId",
                table: "EmployeAffectationResidences",
                newName: "IX_EmployeAffectationResidences_UserId_ResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeAffectationResidences_AspNetUsers_UserId",
                table: "EmployeAffectationResidences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeAffectationResidences_AspNetUsers_UserId",
                table: "EmployeAffectationResidences");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmployeAffectationResidences",
                newName: "EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeAffectationResidences_UserId_ResidenceId",
                table: "EmployeAffectationResidences",
                newName: "IX_EmployeAffectationResidences_EmployeId_ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeAffectationResidences_EmployeId_ResidenceId_DateDebut",
                table: "EmployeAffectationResidences",
                columns: new[] { "EmployeId", "ResidenceId", "DateDebut" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeAffectationResidences_Employes_EmployeId",
                table: "EmployeAffectationResidences",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
