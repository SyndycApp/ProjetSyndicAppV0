using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyToMessageId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageReactions",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReactions", x => new { x.MessageId, x.UserId, x.Emoji });
                    table.ForeignKey(
                        name: "FK_MessageReactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReactions_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReplyToMessageId",
                table: "Messages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_UserId",
                table: "MessageReactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ReplyToMessageId",
                table: "Messages",
                column: "ReplyToMessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ReplyToMessageId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageReactions");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReplyToMessageId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReplyToMessageId",
                table: "Messages");
        }
    }
}
