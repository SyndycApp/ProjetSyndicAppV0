using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.AppelVocal;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.LocauxCommerciaux;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Infrastructure.Identity;
using System;

namespace SyndicApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Résidences
        public DbSet<Residence> Residences => Set<Residence>();
        public DbSet<Batiment> Batiments => Set<Batiment>();
        public DbSet<Lot> Lots => Set<Lot>();
        public DbSet<AffectationLot> AffectationsLots => Set<AffectationLot>();
        public DbSet<LocataireTemporaire> LocatairesTemporaires => Set<LocataireTemporaire>();

        // Finances
        public DbSet<Charge> Charges => Set<Charge>();
        public DbSet<AppelDeFonds> AppelsDeFonds => Set<AppelDeFonds>();
        public DbSet<Paiement> Paiements => Set<Paiement>();

        // Incidents
        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<DevisTravaux> DevisTravaux => Set<DevisTravaux>();
        public DbSet<Intervention> Interventions => Set<Intervention>();

        public DbSet<Call> Calls { get; set; }

        public DbSet<MessageReaction> MessageReactions => Set<MessageReaction>();


        public DbSet<IncidentHistorique> IncidentsHistoriques => Set<IncidentHistorique>();
        public DbSet<DevisHistorique> DevisHistoriques => Set<DevisHistorique>();
        public DbSet<InterventionHistorique> InterventionsHistoriques => Set<InterventionHistorique>();

        // Personnel
        public DbSet<Employe> Employes => Set<Employe>();
        public DbSet<OffreEmploi> OffresEmploi => Set<OffreEmploi>();
        public DbSet<Candidature> Candidatures => Set<Candidature>();

        // Annonces
        public DbSet<Annonce> Annonces => Set<Annonce>();
        public DbSet<CategorieAnnonce> CategoriesAnnonces => Set<CategorieAnnonce>();

        // Assemblées
        public DbSet<AssembleeGenerale> AssembleesGenerales => Set<AssembleeGenerale>();
        public DbSet<Convocation> Convocations => Set<Convocation>();
        public DbSet<ConvocationDestinataire> ConvocationDestinataires => Set<ConvocationDestinataire>();
        public DbSet<ProcesVerbal> ProcesVerbaux => Set<ProcesVerbal>();
        public DbSet<Resolution> Resolutions => Set<Resolution>();
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<Decision> Decisions => Set<Decision>();
        public DbSet<Procuration> Procurations => Set<Procuration>();

        public DbSet<PresenceAss> PresenceAss => Set<PresenceAss>();

        // Documents
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<CategorieDocument> CategoriesDocuments => Set<CategorieDocument>();

        // Communication (MESSAGERIE)
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<UserConversation> UserConversations => Set<UserConversation>();

        // Locaux commerciaux
        public DbSet<LocalCommercial> LocauxCommerciaux { get; set; }
        public DbSet<ActiviteCommerciale> ActivitesCommerciales => Set<ActiviteCommerciale>();

        // Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<Prestataire> Prestataires { get; set; } = default!;

        public DbSet<Presence> Presences => Set<Presence>();

        public DbSet<EmployeProfil> EmployeProfils => Set<EmployeProfil>();
        public DbSet<HoraireTheorique> HorairesTheoriques => Set<HoraireTheorique>();
        public DbSet<MissionEmploye> MissionsEmployes => Set<MissionEmploye>();

        public DbSet<HoraireTravail> HorairesTravail => Set<HoraireTravail>();
        public DbSet<DocumentRH> DocumentsRH => Set<DocumentRH>();
        public DbSet<EmployeAffectationResidence> EmployeAffectationResidences => Set<EmployeAffectationResidence>();
        public DbSet<PlanningMission> PlanningMissions => Set<PlanningMission>();

        public DbSet<MissionValidation> MissionValidations => Set<MissionValidation>();

        public DbSet<OrdreDuJourItem> OrdreDuJour => Set<OrdreDuJourItem>();

        public DbSet<AbsenceJustification> AbsenceJustifications => Set<AbsenceJustification>();

        public DbSet<EmployeDocument> EmployeDocuments => Set<EmployeDocument>();

        public DbSet<PersonnelScoreHistorique> PersonnelScoreHistoriques => Set<PersonnelScoreHistorique>();

        public DbSet<ResidencePlanningConfig> ResidencePlanningConfigs => Set<ResidencePlanningConfig>();

        public DbSet<PrestataireNote> PrestataireNotes => Set<PrestataireNote>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<ConvocationPieceJointe> ConvocationPiecesJointes => Set<ConvocationPieceJointe>();

        public DbSet<ModeleConvocation> ModelesConvocation => Set<ModeleConvocation>();

        public DbSet<ConvocationEnvoiLog> ConvocationEnvoiLogs => Set<ConvocationEnvoiLog>();

        public DbSet<AssembleeRappel> AssembleeRappels => Set<AssembleeRappel>();

        public DbSet<ProcesVerbalVersion> ProcesVerbalVersions => Set<ProcesVerbalVersion>();

        public DbSet<AnnotationAssemblee> AnnotationsAssemblee => Set<AnnotationAssemblee>();

        public DbSet<RelanceVoteLog> RelanceVoteLogs => Set<RelanceVoteLog>();




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProcesVerbalVersion>(entity =>
            {
                entity.ToTable("ProcesVerbalVersions");

                entity.HasKey(v => v.Id);

                entity.Property(v => v.NumeroVersion)
                    .IsRequired();

                entity.Property(v => v.Contenu)
                    .IsRequired();

                entity.Property(v => v.UrlPdf)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(v => v.EstOfficielle)
                    .IsRequired();

                entity.Property(v => v.DateGeneration)
                    .IsRequired();

                entity.Property(v => v.GenereParId)
                    .IsRequired();

                // 🔗 Relation avec ProcesVerbal (1 -> N)
                entity.HasOne(v => v.ProcesVerbal)
                    .WithMany(pv => pv.Versions)
                    .HasForeignKey(v => v.ProcesVerbalId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 🔒 Une seule version officielle par PV
                entity.HasIndex(v => new { v.ProcesVerbalId, v.EstOfficielle })
                    .HasFilter("[EstOfficielle] = 1")
                    .IsUnique();

                // 🔎 Index utile pour l’historique
                entity.HasIndex(v => new { v.ProcesVerbalId, v.NumeroVersion })
                    .IsUnique();
            });

            modelBuilder.Entity<RelanceVoteLog>(entity =>
            {
                entity.ToTable("RelanceVoteLogs");

                entity.HasKey(r => r.Id);

                entity.HasIndex(r => new { r.AssembleeGeneraleId, r.UserId, r.Type })
                    .IsUnique();

                entity.Property(r => r.Type)
                    .HasMaxLength(50)
                    .IsRequired();
            });


            modelBuilder.Entity<AnnotationAssemblee>(entity =>
            {
                entity.ToTable("AnnotationAssemblees");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.Contenu)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(a => a.DateCreation)
                    .IsRequired();

                entity.HasOne(a => a.AssembleeGenerale)
                    .WithMany()
                    .HasForeignKey(a => a.AssembleeGeneraleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(a => a.AssembleeGeneraleId);
            });


            // ================= Résidences / Bâtiments / Lots =================
            modelBuilder.Entity<Residence>(b =>
            {
                b.Property(x => x.Nom).HasMaxLength(200).IsRequired();
                b.Property(x => x.Adresse).HasMaxLength(300);
                b.Property(x => x.Ville).HasMaxLength(150);
                b.Property(x => x.CodePostal).HasMaxLength(20);
            });

            modelBuilder.Entity<EmployeDocument>(b =>
            {
                b.Property(d => d.Type)
                 .HasConversion<string>()
                 .HasMaxLength(50)
                 .IsRequired();

                b.Property(d => d.FilePath)
                 .HasMaxLength(500)
                 .IsRequired();

                b.HasOne(d => d.Employe)
                 .WithMany()
                 .HasForeignKey(d => d.EmployeId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PersonnelScoreHistorique>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeId)
                    .IsRequired();

                entity.Property(e => e.Annee)
                    .IsRequired();

                entity.Property(e => e.Mois)
                    .IsRequired();


                entity.Property(e => e.ScoreBrut)
                    .IsRequired()
                    .HasPrecision(5, 2); 

                entity.Property(e => e.ScoreNormalise)
                    .IsRequired();

                entity.HasIndex(e => new { e.EmployeId, e.Annee, e.Mois })
                    .IsUnique();

                entity.HasOne<Employe>()
                    .WithMany()
                    .HasForeignKey(e => e.EmployeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ConvocationPieceJointe>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.NomFichier)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(p => p.UrlFichier)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(p => p.Convocation)
                    .WithMany(c => c.PiecesJointes)
                    .HasForeignKey(p => p.ConvocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ConvocationEnvoiLog>(entity =>
            {
                entity.HasOne(e => e.Convocation)
                    .WithMany(c => c.Envois)
                    .HasForeignKey(e => e.ConvocationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);
            });
            modelBuilder.Entity<ModeleConvocation>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Nom)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(m => m.Contenu)
                    .IsRequired();

                entity.Property(m => m.EstParDefaut)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<PlanningMission>()
                        .HasOne(m => m.Validation)
                        .WithOne(v => v.PlanningMission)
                        .HasForeignKey<MissionValidation>(v => v.PlanningMissionId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Batiment>().ToTable("Batiment");
            modelBuilder.Entity<Batiment>()
                .HasOne(b => b.Residence)
                .WithMany()
                .HasForeignKey(b => b.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdreDuJourItem>()
                        .HasOne(o => o.AssembleeGenerale)
                        .WithMany(a => a.OrdreDuJour)
                        .HasForeignKey(o => o.AssembleeGeneraleId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdreDuJourItem>()
                        .HasIndex(o => new { o.AssembleeGeneraleId, o.Ordre })
                        .IsUnique();


            modelBuilder.Entity<Decision>(b =>
            {
                b.HasOne(d => d.Resolution)
                 .WithOne(r => r.Decision)
                 .HasForeignKey<Decision>(d => d.ResolutionId)
                 .OnDelete(DeleteBehavior.Restrict); 

                b.HasIndex(d => d.ResolutionId)
                 .IsUnique();
            });

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Residence)
                .WithMany(r => r.Lots)
                .HasForeignKey(l => l.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Lot>()
                        .Property(l => l.Tantiemes)
                        .HasPrecision(10, 2)
                        .IsRequired();

            modelBuilder.Entity<LocataireTemporaire>()
                .HasOne(lt => lt.Lot)
                .WithMany(l => l.LocationsTemporaires)
                .HasForeignKey(lt => lt.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrestataireNote>(b =>
            {
                b.ToTable("PrestataireNotes");
                b.HasKey(n => n.Id);
                b.Property(n => n.Qualite)
                    .IsRequired();
                b.Property(n => n.Delai)
                    .IsRequired();
                b.Property(n => n.Communication)
                    .IsRequired();
                b.HasIndex(n => new { n.PrestataireId, n.AuteurSyndicId })
                    .IsUnique();
            });

            modelBuilder.Entity<ResidencePlanningConfig>(b =>
            {
                b.HasOne(c => c.Residence)
                 .WithMany()
                 .HasForeignKey(c => c.ResidenceId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(c => c.ResidenceId).IsUnique();
            });
            modelBuilder.Entity<PlanningMission>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Employe)
                    .WithMany()
                    .HasForeignKey(p => p.EmployeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Residence)
                    .WithMany()
                    .HasForeignKey(p => p.ResidenceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Mission)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Statut)
                    .HasMaxLength(50)
                    .HasDefaultValue("Planifiee");

                entity.HasIndex(p => new { p.EmployeId, p.Date });
            });


            modelBuilder.Entity<EmployeAffectationResidence>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Residence)
                    .WithMany()
                    .HasForeignKey(e => e.ResidenceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.UserId, e.ResidenceId })
                    .HasFilter("[DateFin] IS NULL")
                    .IsUnique();
            });

            modelBuilder.Entity<AbsenceJustification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasConversion(
                        d => d.ToDateTime(TimeOnly.MinValue),
                        d => DateOnly.FromDateTime(d)
                    );
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20);
                entity.Property(e => e.Motif)
                    .HasMaxLength(500);
                entity.Property(e => e.DocumentUrl)
                    .HasMaxLength(500);
                entity.Property(e => e.Validee)
                    .HasDefaultValue(false);
                entity.HasIndex(e => new { e.UserId, e.Date })
                    .IsUnique();
            });

            // ================= AffectationLot =================
            modelBuilder.Entity<AffectationLot>(b =>
            {
                b.ToTable("AffectationsLots");

                b.HasOne(a => a.Lot)
                 .WithMany(l => l.Affectations)
                 .HasForeignKey(a => a.LotId)
                 .HasConstraintName("FK_AffectationsLots_Lots_LotId")
                 .OnDelete(DeleteBehavior.Cascade);

                b.Property(a => a.UserId).HasColumnName("UserId").IsRequired();

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .HasPrincipalKey(u => u.Id)
                 .HasConstraintName("FK_AffectationsLots_AspNetUsers_UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ==================== MESSAGERIE ====================
            modelBuilder.Entity<UserConversation>(b =>
            {
                // Clé composite
                b.HasKey(uc => new { uc.UserId, uc.ConversationId });

                // Relation avec AspNetUsers
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(uc => uc.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Relation Conversation
                b.HasOne(uc => uc.Conversation)
                 .WithMany(c => c.UserConversations)
                 .HasForeignKey(uc => uc.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(b =>
            {
                // Relation Conversation
                b.HasOne(m => m.Conversation)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(m => m.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(m => m.ReplyToMessage)
                 .WithMany()
                 .HasForeignKey(m => m.ReplyToMessageId)
                 .OnDelete(DeleteBehavior.NoAction);

                // Relation avec AspNetUsers
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(m => m.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Valeur par défaut CreatedAt
                b.Property(m => m.CreatedAt)
                 .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<MessageReaction>(b =>
            {
                // ✅ 1 réaction max par utilisateur et par message
                b.HasKey(x => new { x.MessageId, x.UserId });

                b.Property(x => x.Emoji)
                 .IsRequired();

                b.HasOne(x => x.Message)
                 .WithMany(m => m.Reactions)
                 .HasForeignKey(x => x.MessageId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PresenceAss>(b =>
            {
                b.HasIndex(p => new { p.AssembleeGeneraleId, p.UserId })
                 .IsUnique();
            });

            modelBuilder.Entity<Procuration>(b =>
            {
                b.HasIndex(p => new { p.AssembleeGeneraleId, p.DonneurId })
                 .IsUnique();
            });

            // ================= Finances =================
            modelBuilder.Entity<AppelDeFonds>()
                .Property(a => a.MontantTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AppelDeFonds>()
                .Property<bool>("EstCloture")
                .HasDefaultValue(false);

            modelBuilder.Entity<Paiement>(b =>
            {
                b.Property(p => p.Montant).HasPrecision(18, 2);

                b.HasOne(p => p.AppelDeFonds)
                 .WithMany(a => a.Paiements)
                 .HasForeignKey(p => p.AppelDeFondsId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Charge>(b =>
            {
                b.Property(c => c.Montant).HasPrecision(18, 2);

                b.HasOne(c => c.Residence)
                 .WithMany()
                 .HasForeignKey(c => c.ResidenceId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(c => c.Lot)
                 .WithMany()
                 .HasForeignKey(c => c.LotId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ================= Incidents =================
            modelBuilder.Entity<Incident>(b =>
            {
                b.ToTable("Incidents");

                b.Property(x => x.Titre).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).IsRequired();
                b.Property(x => x.TypeIncident).HasMaxLength(100);

                b.Property(x => x.Statut).IsRequired();
                b.Property(x => x.Urgence).IsRequired();

                b.Property(x => x.DateDeclaration).IsRequired();

                b.HasIndex(x => new { x.ResidenceId, x.Statut });
                b.HasIndex(x => x.DateDeclaration);
                b.HasIndex(x => x.DeclareParId);

                b.HasOne(x => x.Residence)
                 .WithMany(r => r.Incidents)
                 .HasForeignKey(x => x.ResidenceId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Lot)
                 .WithMany(l => l.Incidents)
                 .HasForeignKey(x => x.LotId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(x => x.DeclareParId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.Interventions)
                 .WithOne(i => i.Incident)
                 .HasForeignKey(i => i.IncidentId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.Devis)
                 .WithOne(d => d.Incident)
                 .HasForeignKey(d => d.IncidentId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.Historique)
                 .WithOne(h => h.Incident)
                 .HasForeignKey(h => h.IncidentId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(x => x.Documents)
                 .WithMany()
                 .UsingEntity<Dictionary<string, object>>(
                    "IncidentDocuments",
                    right => right.HasOne<Document>()
                                  .WithMany()
                                  .HasForeignKey("DocumentId")
                                  .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<Incident>()
                                 .WithMany()
                                 .HasForeignKey("IncidentId")
                                 .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("IncidentDocuments");
                        join.HasKey("IncidentId", "DocumentId");
                        join.HasIndex("DocumentId");
                    });
            });

            // ================= DevisTravaux =================
            modelBuilder.Entity<DevisTravaux>(b =>
            {
                b.ToTable("DevisTravaux");

                b.Property(x => x.Titre).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).IsRequired();

                b.Property(x => x.MontantHT).HasPrecision(18, 2);
                b.Property(x => x.TauxTVA).HasPrecision(5, 4);
                b.Property(x => x.DateEmission).IsRequired();
                b.Property(x => x.Statut).IsRequired();

                b.HasIndex(x => new { x.ResidenceId, x.Statut });
                b.HasIndex(x => x.DateEmission);
                b.HasIndex(x => x.ValideParId);
                b.HasIndex(x => x.DateDecision);

                b.HasOne(x => x.Residence)
                 .WithMany(r => r.DevisTravaux)
                 .HasForeignKey(x => x.ResidenceId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Incident)
                 .WithMany(i => i.Devis)
                 .HasForeignKey(x => x.IncidentId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.Interventions)
                 .WithOne(i => i.DevisTravaux)
                 .HasForeignKey(i => i.DevisTravauxId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.Historique)
                 .WithOne(h => h.DevisTravaux)
                 .HasForeignKey(h => h.DevisTravauxId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(x => x.Documents)
                 .WithMany()
                 .UsingEntity<Dictionary<string, object>>(
                    "DevisDocuments",
                    right => right.HasOne<Document>()
                                  .WithMany()
                                  .HasForeignKey("DocumentId")
                                  .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<DevisTravaux>()
                                 .WithMany()
                                 .HasForeignKey("DevisTravauxId")
                                 .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("DevisDocuments");
                        join.HasKey("DevisTravauxId", "DocumentId");
                        join.HasIndex("DocumentId");
                    });
            });

            // ================= Interventions =================
            modelBuilder.Entity<Intervention>(b =>
            {
                b.ToTable("Interventions");

                b.Property(x => x.Description).IsRequired();
                b.Property(x => x.CoutEstime).HasPrecision(18, 2);
                b.Property(x => x.CoutReel).HasPrecision(18, 2);
                b.Property(x => x.Statut).IsRequired();

                b.HasIndex(x => new { x.ResidenceId, x.Statut });
                b.HasIndex(x => x.DatePrevue);
                b.HasIndex(x => x.DateRealisation);
                b.HasIndex(x => x.EmployeId);

                b.HasOne(x => x.Residence)
                 .WithMany(r => r.Interventions)
                 .HasForeignKey(x => x.ResidenceId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.DevisTravaux)
                 .WithMany(d => d.Interventions)
                 .HasForeignKey(x => x.DevisTravauxId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.Incident)
                 .WithMany(i => i.Interventions)
                 .HasForeignKey(x => x.IncidentId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.Employe)
                 .WithMany(e => e.Interventions)
                 .HasForeignKey(x => x.EmployeId)
                 .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.Documents)
                 .WithMany()
                 .UsingEntity<Dictionary<string, object>>(
                    "InterventionDocuments",
                    right => right.HasOne<Document>()
                                  .WithMany()
                                  .HasForeignKey("DocumentId")
                                  .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<Intervention>()
                                 .WithMany()
                                 .HasForeignKey("InterventionId")
                                 .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.ToTable("InterventionDocuments");
                        join.HasKey("InterventionId", "DocumentId");
                        join.HasIndex("DocumentId");
                    });

                b.HasMany(x => x.Historique)
                 .WithOne(h => h.Intervention)
                 .HasForeignKey(h => h.InterventionId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= Prestataires =================
            modelBuilder.Entity<Prestataire>(b =>
            {
                b.ToTable("Prestataires");

                b.Property(p => p.Nom)
                 .HasMaxLength(200)
                 .IsRequired();

                b.Property(p => p.TypeService).HasMaxLength(200);
                b.Property(p => p.Email).HasMaxLength(200);
                b.Property(p => p.Telephone).HasMaxLength(50);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(p => p.UserId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ================= Historiques =================
            modelBuilder.Entity<IncidentHistorique>(b =>
            {
                b.ToTable("IncidentsHistoriques");

                b.Property(x => x.Action).HasMaxLength(250).IsRequired();
                b.Property(x => x.Commentaire).HasMaxLength(1000);

                b.HasIndex(x => x.IncidentId);
                b.HasIndex(x => x.DateAction);
                b.HasIndex(x => x.AuteurId);

                b.HasOne(x => x.Incident)
                 .WithMany(i => i.Historique)
                 .HasForeignKey(x => x.IncidentId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DevisHistorique>(b =>
            {
                b.ToTable("DevisHistoriques");

                b.Property(x => x.Action).HasMaxLength(250).IsRequired();
                b.Property(x => x.Commentaire).HasMaxLength(1000);

                b.HasIndex(x => x.DevisTravauxId);
                b.HasIndex(x => x.DateAction);
                b.HasIndex(x => x.AuteurId);

                b.HasOne(x => x.DevisTravaux)
                 .WithMany(d => d.Historique)
                 .HasForeignKey(x => x.DevisTravauxId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InterventionHistorique>(b =>
            {
                b.ToTable("InterventionsHistoriques");

                b.Property(x => x.Action).HasMaxLength(250).IsRequired();
                b.Property(x => x.Commentaire).HasMaxLength(1000);

                b.HasIndex(x => x.InterventionId);
                b.HasIndex(x => x.DateAction);
                b.HasIndex(x => x.AuteurId);

                b.HasOne(x => x.Intervention)
                 .WithMany(i => i.Historique)
                 .HasForeignKey(x => x.InterventionId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Document>(b =>
            {
                b.HasOne(d => d.Categorie)
                 .WithMany(c => c.Documents)
                 .HasForeignKey(d => d.CategorieId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(d => d.AssembleeGenerale)
                 .WithMany(ag => ag.Documents)
                 .HasForeignKey(d => d.AssembleeGeneraleId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(d => d.Residence)
                 .WithMany(r => r.Documents)
                 .HasForeignKey(d => d.ResidenceId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(d => d.AjouteParId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Annonce>(b =>
            {
                b.HasOne(a => a.Categorie)
                 .WithMany(c => c.Annonces)
                 .HasForeignKey(a => a.CategorieId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(a => a.Residence)
                 .WithMany(r => r.Annonces)
                 .HasForeignKey(a => a.ResidenceId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= Assemblées =================
            modelBuilder.Entity<Vote>(b =>
            {
                b.HasOne(v => v.Resolution)
                 .WithMany(r => r.Votes)
                 .HasForeignKey(v => v.ResolutionId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(v => new { v.ResolutionId, v.UserId })
                 .IsUnique()
                 .HasDatabaseName("UX_Vote_ParResolution");
            });

            modelBuilder.Entity<Resolution>(b =>
            {
                b.HasOne(r => r.AssembleeGenerale)
                 .WithMany(a => a.Resolutions)
                 .HasForeignKey(r => r.AssembleeGeneraleId);
            });

            modelBuilder.Entity<ConvocationDestinataire>(b =>
            {
                b.HasOne(d => d.Convocation)
                 .WithMany(c => c.Destinataires)
                 .HasForeignKey(d => d.ConvocationId);
            });

            modelBuilder.Entity<ProcesVerbal>(b =>
            {
                b.HasOne(p => p.AssembleeGenerale)
                 .WithOne(a => a.ProcesVerbal)
                 .HasForeignKey<ProcesVerbal>(p => p.AssembleeGeneraleId);
            });

            // ================= Appels Vocaux =================
            modelBuilder.Entity<Call>(b =>
            {
                b.ToTable("Calls");

                b.Property(c => c.CallerId).IsRequired();
                b.Property(c => c.ReceiverId).IsRequired();
                b.Property(c => c.StartedAt).IsRequired();

                b.Property(c => c.Status)
                 .HasConversion<int>()
                 .IsRequired();

                b.HasIndex(c => c.CallerId);
                b.HasIndex(c => c.ReceiverId);
                b.HasIndex(c => c.StartedAt);
                b.HasIndex(c => c.Status);
            });


            modelBuilder.Entity<LocalCommercial>(b =>
            {
                b.ToTable("LocauxCommerciaux");

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(l => l.ProprietaireId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(l => l.LocataireId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(l => l.Activite)
                 .WithMany()
                 .HasForeignKey(l => l.ActiviteId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(l => l.Lot)
                 .WithMany()
                 .HasForeignKey(l => l.LotId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Call>()
                        .Property(c => c.EndedAt)
                        .IsRequired(false);

            modelBuilder.Entity<Notification>(b =>
            {
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Employe>(b =>
            {
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
