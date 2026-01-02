using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableAssb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdreDuJourItem_AssembleesGenerales_AssembleeGeneraleId",
                table: "OrdreDuJourItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdreDuJourItem",
                table: "OrdreDuJourItem");

            migrationBuilder.DropIndex(
                name: "IX_OrdreDuJourItem_AssembleeGeneraleId",
                table: "OrdreDuJourItem");

            migrationBuilder.RenameTable(
                name: "OrdreDuJourItem",
                newName: "OrdreDuJour");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdreDuJour",
                table: "OrdreDuJour",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdreDuJour_AssembleeGeneraleId_Ordre",
                table: "OrdreDuJour",
                columns: new[] { "AssembleeGeneraleId", "Ordre" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdreDuJour_AssembleesGenerales_AssembleeGeneraleId",
                table: "OrdreDuJour",
                column: "AssembleeGeneraleId",
                principalTable: "AssembleesGenerales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdreDuJour_AssembleesGenerales_AssembleeGeneraleId",
                table: "OrdreDuJour");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdreDuJour",
                table: "OrdreDuJour");

            migrationBuilder.DropIndex(
                name: "IX_OrdreDuJour_AssembleeGeneraleId_Ordre",
                table: "OrdreDuJour");

            migrationBuilder.RenameTable(
                name: "OrdreDuJour",
                newName: "OrdreDuJourItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdreDuJourItem",
                table: "OrdreDuJourItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdreDuJourItem_AssembleeGeneraleId",
                table: "OrdreDuJourItem",
                column: "AssembleeGeneraleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdreDuJourItem_AssembleesGenerales_AssembleeGeneraleId",
                table: "OrdreDuJourItem",
                column: "AssembleeGeneraleId",
                principalTable: "AssembleesGenerales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
