using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrestataireEntityAndRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrestataireExterne",
                table: "Interventions");

            migrationBuilder.AddColumn<Guid>(
                name: "PrestataireId",
                table: "Interventions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Prestataires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestataires", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_PrestataireId",
                table: "Interventions",
                column: "PrestataireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions",
                column: "PrestataireId",
                principalTable: "Prestataires",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions");

            migrationBuilder.DropTable(
                name: "Prestataires");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_PrestataireId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "PrestataireId",
                table: "Interventions");

            migrationBuilder.AddColumn<string>(
                name: "PrestataireExterne",
                table: "Interventions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
