using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TabN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeProfilId",
                table: "MissionsEmployes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HorairesTravail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeProfilId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Jour = table.Column<int>(type: "int", nullable: false),
                    HeureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorairesTravail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorairesTravail_EmployeProfils_EmployeProfilId",
                        column: x => x.EmployeProfilId,
                        principalTable: "EmployeProfils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionsEmployes_EmployeProfilId",
                table: "MissionsEmployes",
                column: "EmployeProfilId");

            migrationBuilder.CreateIndex(
                name: "IX_HorairesTravail_EmployeProfilId",
                table: "HorairesTravail",
                column: "EmployeProfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionsEmployes_EmployeProfils_EmployeProfilId",
                table: "MissionsEmployes",
                column: "EmployeProfilId",
                principalTable: "EmployeProfils",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionsEmployes_EmployeProfils_EmployeProfilId",
                table: "MissionsEmployes");

            migrationBuilder.DropTable(
                name: "HorairesTravail");

            migrationBuilder.DropIndex(
                name: "IX_MissionsEmployes_EmployeProfilId",
                table: "MissionsEmployes");

            migrationBuilder.DropColumn(
                name: "EmployeProfilId",
                table: "MissionsEmployes");
        }
    }
}
