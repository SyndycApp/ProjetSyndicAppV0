using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveOccupantPerLot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_UsersApp_UserId",
                table: "AffectationsLots");

            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_UsersApp_UserId1",
                table: "AffectationsLots");

            migrationBuilder.DropForeignKey(
                name: "FK_Annonces_UsersApp_UserId",
                table: "Annonces");

            migrationBuilder.DropForeignKey(
                name: "FK_Convocations_UsersApp_UserId",
                table: "Convocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_UsersApp_AjouteParId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_UsersApp_DeclareParId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocauxCommerciaux_UsersApp_LocataireId",
                table: "LocauxCommerciaux");

            migrationBuilder.DropForeignKey(
                name: "FK_LocauxCommerciaux_UsersApp_ProprietaireId",
                table: "LocauxCommerciaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UsersApp_ExpediteurId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UsersApp_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Paiements_UsersApp_UserId",
                table: "Paiements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_UsersApp_UserId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersApp_Role_RoleId",
                table: "UsersApp");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_UsersApp_UserId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersApp",
                table: "UsersApp");

            migrationBuilder.RenameTable(
                name: "UsersApp",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_UsersApp_RoleId",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AffectationsLots",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UX_Lot_OccupantActif",
                table: "AffectationsLots",
                column: "LotId",
                unique: true,
                filter: "[DateFin] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_UserId",
                table: "AffectationsLots",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_User_UserId1",
                table: "AffectationsLots",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Annonces_User_UserId",
                table: "Annonces",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Convocations_User_UserId",
                table: "Convocations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_User_AjouteParId",
                table: "Documents",
                column: "AjouteParId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_User_DeclareParId",
                table: "Incidents",
                column: "DeclareParId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocauxCommerciaux_User_LocataireId",
                table: "LocauxCommerciaux",
                column: "LocataireId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocauxCommerciaux_User_ProprietaireId",
                table: "LocauxCommerciaux",
                column: "ProprietaireId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_User_ExpediteurId",
                table: "Messages",
                column: "ExpediteurId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_User_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_User_UserId",
                table: "Paiements",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_User_UserId",
                table: "UserConversations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_User_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_AspNetUsers_UserId",
                table: "AffectationsLots");

            migrationBuilder.DropForeignKey(
                name: "FK_AffectationsLots_User_UserId1",
                table: "AffectationsLots");

            migrationBuilder.DropForeignKey(
                name: "FK_Annonces_User_UserId",
                table: "Annonces");

            migrationBuilder.DropForeignKey(
                name: "FK_Convocations_User_UserId",
                table: "Convocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_User_AjouteParId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_User_DeclareParId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_LocauxCommerciaux_User_LocataireId",
                table: "LocauxCommerciaux");

            migrationBuilder.DropForeignKey(
                name: "FK_LocauxCommerciaux_User_ProprietaireId",
                table: "LocauxCommerciaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_User_ExpediteurId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_User_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Paiements_User_UserId",
                table: "Paiements");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_User_UserId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_User_UserId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "UX_Lot_OccupantActif",
                table: "AffectationsLots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UsersApp");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "UsersApp",
                newName: "IX_UsersApp_RoleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AffectationsLots",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersApp",
                table: "UsersApp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_UsersApp_UserId",
                table: "AffectationsLots",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AffectationsLots_UsersApp_UserId1",
                table: "AffectationsLots",
                column: "UserId1",
                principalTable: "UsersApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Annonces_UsersApp_UserId",
                table: "Annonces",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Convocations_UsersApp_UserId",
                table: "Convocations",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_UsersApp_AjouteParId",
                table: "Documents",
                column: "AjouteParId",
                principalTable: "UsersApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_UsersApp_DeclareParId",
                table: "Incidents",
                column: "DeclareParId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocauxCommerciaux_UsersApp_LocataireId",
                table: "LocauxCommerciaux",
                column: "LocataireId",
                principalTable: "UsersApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocauxCommerciaux_UsersApp_ProprietaireId",
                table: "LocauxCommerciaux",
                column: "ProprietaireId",
                principalTable: "UsersApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UsersApp_ExpediteurId",
                table: "Messages",
                column: "ExpediteurId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UsersApp_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_UsersApp_UserId",
                table: "Paiements",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_UsersApp_UserId",
                table: "UserConversations",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersApp_Role_RoleId",
                table: "UsersApp",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_UsersApp_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "UsersApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
