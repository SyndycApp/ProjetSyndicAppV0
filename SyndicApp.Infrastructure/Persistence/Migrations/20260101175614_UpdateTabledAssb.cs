using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabledAssb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LuLe",
                table: "ConvocationDestinataires",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RelanceLe",
                table: "ConvocationDestinataires",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConvocationPiecesJointes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConvocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomFichier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UrlFichier = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationPiecesJointes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationPiecesJointes_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelesConvocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contenu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstParDefaut = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelesConvocation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationPiecesJointes_ConvocationId",
                table: "ConvocationPiecesJointes",
                column: "ConvocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvocationPiecesJointes");

            migrationBuilder.DropTable(
                name: "ModelesConvocation");

            migrationBuilder.DropColumn(
                name: "LuLe",
                table: "ConvocationDestinataires");

            migrationBuilder.DropColumn(
                name: "RelanceLe",
                table: "ConvocationDestinataires");
        }
    }
}
