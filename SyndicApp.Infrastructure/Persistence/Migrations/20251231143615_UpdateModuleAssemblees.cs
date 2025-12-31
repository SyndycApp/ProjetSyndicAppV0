using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModuleAssemblees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Convocations_AspNetUsers_UserId",
                table: "Convocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AssembleesGenerales_AssembleeGeneraleId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Convocations_UserId",
                table: "Convocations");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "EstLu",
                table: "Convocations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Convocations");

            migrationBuilder.RenameColumn(
                name: "AssembleeGeneraleId",
                table: "Votes",
                newName: "ResolutionId");

            migrationBuilder.RenameIndex(
                name: "UX_Vote_UniqueParAG",
                table: "Votes",
                newName: "UX_Vote_ParResolution");

            migrationBuilder.RenameColumn(
                name: "Lieu",
                table: "AssembleesGenerales",
                newName: "Titre");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "AssembleesGenerales",
                newName: "DateFin");

            migrationBuilder.RenameColumn(
                name: "Cloturee",
                table: "AssembleesGenerales",
                newName: "EstArchivee");

            migrationBuilder.AlterColumn<int>(
                name: "Choix",
                table: "Votes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "EstModifie",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LotId",
                table: "Votes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "PoidsVote",
                table: "Votes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Contenu",
                table: "Convocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Annee",
                table: "AssembleesGenerales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreeParId",
                table: "AssembleesGenerales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCloture",
                table: "AssembleesGenerales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "AssembleesGenerales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ResidenceId",
                table: "AssembleesGenerales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "AssembleesGenerales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AssembleesGenerales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConvocationDestinataires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConvocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstLu = table.Column<bool>(type: "bit", nullable: false),
                    DateLecture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvocationDestinataires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvocationDestinataires_Convocations_ConvocationId",
                        column: x => x.ConvocationId,
                        principalTable: "Convocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcesVerbaux",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssembleeGeneraleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlPdf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateGeneration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstArchive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesVerbaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcesVerbaux_AssembleesGenerales_AssembleeGeneraleId",
                        column: x => x.AssembleeGeneraleId,
                        principalTable: "AssembleesGenerales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resolutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssembleeGeneraleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resolutions_AssembleesGenerales_AssembleeGeneraleId",
                        column: x => x.AssembleeGeneraleId,
                        principalTable: "AssembleesGenerales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssembleesGenerales_ResidenceId",
                table: "AssembleesGenerales",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvocationDestinataires_ConvocationId",
                table: "ConvocationDestinataires",
                column: "ConvocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcesVerbaux_AssembleeGeneraleId",
                table: "ProcesVerbaux",
                column: "AssembleeGeneraleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_AssembleeGeneraleId",
                table: "Resolutions",
                column: "AssembleeGeneraleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssembleesGenerales_Residences_ResidenceId",
                table: "AssembleesGenerales",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Resolutions_ResolutionId",
                table: "Votes",
                column: "ResolutionId",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssembleesGenerales_Residences_ResidenceId",
                table: "AssembleesGenerales");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Resolutions_ResolutionId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "ConvocationDestinataires");

            migrationBuilder.DropTable(
                name: "ProcesVerbaux");

            migrationBuilder.DropTable(
                name: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_AssembleesGenerales_ResidenceId",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "EstModifie",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "PoidsVote",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "Contenu",
                table: "Convocations");

            migrationBuilder.DropColumn(
                name: "Annee",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "CreeParId",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "DateCloture",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "ResidenceId",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "AssembleesGenerales");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AssembleesGenerales");

            migrationBuilder.RenameColumn(
                name: "ResolutionId",
                table: "Votes",
                newName: "AssembleeGeneraleId");

            migrationBuilder.RenameIndex(
                name: "UX_Vote_ParResolution",
                table: "Votes",
                newName: "UX_Vote_UniqueParAG");

            migrationBuilder.RenameColumn(
                name: "Titre",
                table: "AssembleesGenerales",
                newName: "Lieu");

            migrationBuilder.RenameColumn(
                name: "EstArchivee",
                table: "AssembleesGenerales",
                newName: "Cloturee");

            migrationBuilder.RenameColumn(
                name: "DateFin",
                table: "AssembleesGenerales",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "Choix",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstLu",
                table: "Convocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Convocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Convocations_UserId",
                table: "Convocations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Convocations_AspNetUsers_UserId",
                table: "Convocations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AssembleesGenerales_AssembleeGeneraleId",
                table: "Votes",
                column: "AssembleeGeneraleId",
                principalTable: "AssembleesGenerales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
