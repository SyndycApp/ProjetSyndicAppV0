using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMajInTablePVv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "ProcesVerbaux",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HashPdf",
                table: "ProcesVerbalVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RelanceSignatureLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcesVerbalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateRelance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelanceSignatureLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelanceSignatureLogs_ProcesVerbaux_ProcesVerbalId",
                        column: x => x.ProcesVerbalId,
                        principalTable: "ProcesVerbaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignatureProcesVerbals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcesVerbalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdreSignature = table.Column<int>(type: "int", nullable: false),
                    EstObligatoire = table.Column<bool>(type: "bit", nullable: false),
                    EstSigne = table.Column<bool>(type: "bit", nullable: false),
                    DateSignature = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignatureProcesVerbals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignatureProcesVerbals_ProcesVerbaux_ProcesVerbalId",
                        column: x => x.ProcesVerbalId,
                        principalTable: "ProcesVerbaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelanceSignatureLogs_ProcesVerbalId_UserId",
                table: "RelanceSignatureLogs",
                columns: new[] { "ProcesVerbalId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_RelanceSignatureLogs_ProcesVerbalId_UserId_DateRelance",
                table: "RelanceSignatureLogs",
                columns: new[] { "ProcesVerbalId", "UserId", "DateRelance" });

            migrationBuilder.CreateIndex(
                name: "IX_SignatureProcesVerbals_ProcesVerbalId_OrdreSignature",
                table: "SignatureProcesVerbals",
                columns: new[] { "ProcesVerbalId", "OrdreSignature" });

            migrationBuilder.CreateIndex(
                name: "IX_SignatureProcesVerbals_ProcesVerbalId_UserId",
                table: "SignatureProcesVerbals",
                columns: new[] { "ProcesVerbalId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelanceSignatureLogs");

            migrationBuilder.DropTable(
                name: "SignatureProcesVerbals");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "ProcesVerbaux");

            migrationBuilder.DropColumn(
                name: "HashPdf",
                table: "ProcesVerbalVersions");
        }
    }
}
