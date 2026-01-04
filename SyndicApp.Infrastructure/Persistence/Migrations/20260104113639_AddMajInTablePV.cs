using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMajInTablePV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommentaireParId",
                table: "ProcesVerbalVersions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentaireSyndic",
                table: "ProcesVerbalVersions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCommentaire",
                table: "ProcesVerbalVersions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateScellage",
                table: "ProcesVerbalVersions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstScelle",
                table: "ProcesVerbalVersions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HashScellage",
                table: "ProcesVerbalVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ScelleParId",
                table: "ProcesVerbalVersions",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentaireParId",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "CommentaireSyndic",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "DateCommentaire",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "DateScellage",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "EstScelle",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "HashScellage",
                table: "ProcesVerbalVersions");

            migrationBuilder.DropColumn(
                name: "ScelleParId",
                table: "ProcesVerbalVersions");
        }
    }
}
