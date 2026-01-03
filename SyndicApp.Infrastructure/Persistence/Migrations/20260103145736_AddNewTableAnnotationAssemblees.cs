using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableAnnotationAssemblees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnotationAssemblees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssembleeGeneraleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuteurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Contenu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnotationAssemblees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnotationAssemblees_AssembleesGenerales_AssembleeGeneraleId",
                        column: x => x.AssembleeGeneraleId,
                        principalTable: "AssembleesGenerales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnotationAssemblees_AssembleeGeneraleId",
                table: "AnnotationAssemblees",
                column: "AssembleeGeneraleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnotationAssemblees");
        }
    }
}
