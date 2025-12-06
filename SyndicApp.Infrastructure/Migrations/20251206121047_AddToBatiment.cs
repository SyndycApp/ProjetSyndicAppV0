using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddToBatiment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnneeConstruction",
                table: "Batiment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Bloc",
                table: "Batiment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodeAcces",
                table: "Batiment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasAscenseur",
                table: "Batiment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableNom",
                table: "Batiment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnneeConstruction",
                table: "Batiment");

            migrationBuilder.DropColumn(
                name: "Bloc",
                table: "Batiment");

            migrationBuilder.DropColumn(
                name: "CodeAcces",
                table: "Batiment");

            migrationBuilder.DropColumn(
                name: "HasAscenseur",
                table: "Batiment");

            migrationBuilder.DropColumn(
                name: "ResponsableNom",
                table: "Batiment");
        }
    }
}
