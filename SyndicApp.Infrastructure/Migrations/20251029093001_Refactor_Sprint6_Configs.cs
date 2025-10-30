using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyndicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Sprint6_Configs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_AspNetUsers_DeclareParId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Lots_LotId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Residences_ResidenceId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Employes_EmployeId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Incidents_IncidentId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Residences_ResidenceId",
                table: "Interventions");

            migrationBuilder.DropTable(
                name: "EmployeResidence");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ResidenceId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_ResidenceId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "DateIntervention",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "EstEffectuee",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "EstResolu",
                table: "Incidents");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Residences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeId",
                table: "Residences",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LotId1",
                table: "LocauxCommerciaux",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "Interventions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CoutEstime",
                table: "Interventions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CoutReel",
                table: "Interventions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePrevue",
                table: "Interventions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRealisation",
                table: "Interventions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DevisTravauxId",
                table: "Interventions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrestataireExterne",
                table: "Interventions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "Interventions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Titre",
                table: "Incidents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LotId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeIncident",
                table: "Incidents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Urgence",
                table: "Incidents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LotId1",
                table: "Charges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResidenceId1",
                table: "Charges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResidenceId1",
                table: "Batiment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DevisTravaux",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    MontantHT = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TauxTVA = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    ResidenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValideParId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateDecision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommentaireDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevisTravaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevisTravaux_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DevisTravaux_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentDocuments",
                columns: table => new
                {
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentDocuments", x => new { x.IncidentId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_IncidentDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentDocuments_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentsHistoriques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuteurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentsHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentsHistoriques_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterventionDocuments",
                columns: table => new
                {
                    InterventionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionDocuments", x => new { x.InterventionId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_InterventionDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterventionDocuments_Interventions_InterventionId",
                        column: x => x.InterventionId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterventionsHistoriques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterventionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuteurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterventionsHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterventionsHistoriques_Interventions_InterventionId",
                        column: x => x.InterventionId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevisDocuments",
                columns: table => new
                {
                    DevisTravauxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevisDocuments", x => new { x.DevisTravauxId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_DevisDocuments_DevisTravaux_DevisTravauxId",
                        column: x => x.DevisTravauxId,
                        principalTable: "DevisTravaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevisDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevisHistoriques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DevisTravauxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuteurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevisHistoriques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevisHistoriques_DevisTravaux_DevisTravauxId",
                        column: x => x.DevisTravauxId,
                        principalTable: "DevisTravaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Residences_EmployeId",
                table: "Residences",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_LocauxCommerciaux_LotId1",
                table: "LocauxCommerciaux",
                column: "LotId1");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_DatePrevue",
                table: "Interventions",
                column: "DatePrevue");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_DateRealisation",
                table: "Interventions",
                column: "DateRealisation");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_DevisTravauxId",
                table: "Interventions",
                column: "DevisTravauxId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ResidenceId_Statut",
                table: "Interventions",
                columns: new[] { "ResidenceId", "Statut" });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_DateDeclaration",
                table: "Incidents",
                column: "DateDeclaration");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ResidenceId_Statut",
                table: "Incidents",
                columns: new[] { "ResidenceId", "Statut" });

            migrationBuilder.CreateIndex(
                name: "IX_Charges_LotId1",
                table: "Charges",
                column: "LotId1");

            migrationBuilder.CreateIndex(
                name: "IX_Charges_ResidenceId1",
                table: "Charges",
                column: "ResidenceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Batiment_ResidenceId1",
                table: "Batiment",
                column: "ResidenceId1");

            migrationBuilder.CreateIndex(
                name: "IX_DevisDocuments_DocumentId",
                table: "DevisDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisHistoriques_AuteurId",
                table: "DevisHistoriques",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisHistoriques_DateAction",
                table: "DevisHistoriques",
                column: "DateAction");

            migrationBuilder.CreateIndex(
                name: "IX_DevisHistoriques_DevisTravauxId",
                table: "DevisHistoriques",
                column: "DevisTravauxId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisTravaux_DateDecision",
                table: "DevisTravaux",
                column: "DateDecision");

            migrationBuilder.CreateIndex(
                name: "IX_DevisTravaux_DateEmission",
                table: "DevisTravaux",
                column: "DateEmission");

            migrationBuilder.CreateIndex(
                name: "IX_DevisTravaux_IncidentId",
                table: "DevisTravaux",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisTravaux_ResidenceId_Statut",
                table: "DevisTravaux",
                columns: new[] { "ResidenceId", "Statut" });

            migrationBuilder.CreateIndex(
                name: "IX_DevisTravaux_ValideParId",
                table: "DevisTravaux",
                column: "ValideParId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentDocuments_DocumentId",
                table: "IncidentDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentsHistoriques_AuteurId",
                table: "IncidentsHistoriques",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentsHistoriques_DateAction",
                table: "IncidentsHistoriques",
                column: "DateAction");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentsHistoriques_IncidentId",
                table: "IncidentsHistoriques",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionDocuments_DocumentId",
                table: "InterventionDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionsHistoriques_AuteurId",
                table: "InterventionsHistoriques",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionsHistoriques_DateAction",
                table: "InterventionsHistoriques",
                column: "DateAction");

            migrationBuilder.CreateIndex(
                name: "IX_InterventionsHistoriques_InterventionId",
                table: "InterventionsHistoriques",
                column: "InterventionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batiment_Residences_ResidenceId1",
                table: "Batiment",
                column: "ResidenceId1",
                principalTable: "Residences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Lots_LotId1",
                table: "Charges",
                column: "LotId1",
                principalTable: "Lots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Residences_ResidenceId1",
                table: "Charges",
                column: "ResidenceId1",
                principalTable: "Residences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_AspNetUsers_DeclareParId",
                table: "Incidents",
                column: "DeclareParId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Lots_LotId",
                table: "Incidents",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Residences_ResidenceId",
                table: "Incidents",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_DevisTravaux_DevisTravauxId",
                table: "Interventions",
                column: "DevisTravauxId",
                principalTable: "DevisTravaux",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Employes_EmployeId",
                table: "Interventions",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Incidents_IncidentId",
                table: "Interventions",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Residences_ResidenceId",
                table: "Interventions",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocauxCommerciaux_Lots_LotId1",
                table: "LocauxCommerciaux",
                column: "LotId1",
                principalTable: "Lots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Residences_Employes_EmployeId",
                table: "Residences",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batiment_Residences_ResidenceId1",
                table: "Batiment");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Lots_LotId1",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Residences_ResidenceId1",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_AspNetUsers_DeclareParId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Lots_LotId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Residences_ResidenceId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_DevisTravaux_DevisTravauxId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Employes_EmployeId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Incidents_IncidentId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Residences_ResidenceId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_LocauxCommerciaux_Lots_LotId1",
                table: "LocauxCommerciaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Residences_Employes_EmployeId",
                table: "Residences");

            migrationBuilder.DropTable(
                name: "DevisDocuments");

            migrationBuilder.DropTable(
                name: "DevisHistoriques");

            migrationBuilder.DropTable(
                name: "IncidentDocuments");

            migrationBuilder.DropTable(
                name: "IncidentsHistoriques");

            migrationBuilder.DropTable(
                name: "InterventionDocuments");

            migrationBuilder.DropTable(
                name: "InterventionsHistoriques");

            migrationBuilder.DropTable(
                name: "DevisTravaux");

            migrationBuilder.DropIndex(
                name: "IX_Residences_EmployeId",
                table: "Residences");

            migrationBuilder.DropIndex(
                name: "IX_LocauxCommerciaux_LotId1",
                table: "LocauxCommerciaux");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_DatePrevue",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_DateRealisation",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_DevisTravauxId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ResidenceId_Statut",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_DateDeclaration",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_ResidenceId_Statut",
                table: "Incidents");

            migrationBuilder.DropIndex(
                name: "IX_Charges_LotId1",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Charges_ResidenceId1",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Batiment_ResidenceId1",
                table: "Batiment");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "LotId1",
                table: "LocauxCommerciaux");

            migrationBuilder.DropColumn(
                name: "CoutEstime",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "CoutReel",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "DatePrevue",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "DateRealisation",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "DevisTravauxId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "PrestataireExterne",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "TypeIncident",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Urgence",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "LotId1",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "ResidenceId1",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "ResidenceId1",
                table: "Batiment");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "Interventions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateIntervention",
                table: "Interventions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "EstEffectuee",
                table: "Interventions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Titre",
                table: "Incidents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "ResidenceId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "LotId",
                table: "Incidents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstResolu",
                table: "Incidents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeResidence",
                columns: table => new
                {
                    EmployesAffectesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResidencesAffecteesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeResidence", x => new { x.EmployesAffectesId, x.ResidencesAffecteesId });
                    table.ForeignKey(
                        name: "FK_EmployeResidence_Employes_EmployesAffectesId",
                        column: x => x.EmployesAffectesId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeResidence_Residences_ResidencesAffecteesId",
                        column: x => x.ResidencesAffecteesId,
                        principalTable: "Residences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ResidenceId",
                table: "Interventions",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ResidenceId",
                table: "Incidents",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeResidence_ResidencesAffecteesId",
                table: "EmployeResidence",
                column: "ResidencesAffecteesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_AspNetUsers_DeclareParId",
                table: "Incidents",
                column: "DeclareParId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Lots_LotId",
                table: "Incidents",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Residences_ResidenceId",
                table: "Incidents",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Employes_EmployeId",
                table: "Interventions",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Incidents_IncidentId",
                table: "Interventions",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Residences_ResidenceId",
                table: "Interventions",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id");
        }
    }
}
