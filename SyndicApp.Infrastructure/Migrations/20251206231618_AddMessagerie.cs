using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagerie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_ApplicationUserId3",
                table: "AffectationsLots");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId3",
                table: "AffectationsLots",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AffectationsLots_ApplicationUserId3",
                table: "AffectationsLots",
                newName: "IX_AffectationsLots_ApplicationUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_ApplicationUserId",
                table: "AffectationsLots",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_ApplicationUserId",
                table: "AffectationsLots");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "AffectationsLots",
                newName: "ApplicationUserId3");

            migrationBuilder.RenameIndex(
                name: "IX_AffectationsLots_ApplicationUserId",
                table: "AffectationsLots",
                newName: "IX_AffectationsLots_ApplicationUserId3");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_ApplicationUserId3",
                table: "AffectationsLots",
                column: "ApplicationUserId3",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
