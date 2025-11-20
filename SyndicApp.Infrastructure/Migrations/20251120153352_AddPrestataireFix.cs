using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPrestataireFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions");

            migrationBuilder.AlterColumn<string>(
                name: "TypeService",
                table: "Prestataires",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Prestataires",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Prestataires",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Prestataires",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Prestataires",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prestataires_UserId",
                table: "Prestataires",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions",
                column: "PrestataireId",
                principalTable: "Prestataires",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestataires_AspNetUsers_UserId",
                table: "Prestataires",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestataires_AspNetUsers_UserId",
                table: "Prestataires");

            migrationBuilder.DropIndex(
                name: "IX_Prestataires_UserId",
                table: "Prestataires");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Prestataires");

            migrationBuilder.AlterColumn<string>(
                name: "TypeService",
                table: "Prestataires",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Prestataires",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Prestataires",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Prestataires",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Prestataires_PrestataireId",
                table: "Interventions",
                column: "PrestataireId",
                principalTable: "Prestataires",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
