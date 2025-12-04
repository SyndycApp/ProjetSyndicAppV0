using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.Common;
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
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<Decision> Decisions => Set<Decision>();

        // Documents
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<CategorieDocument> CategoriesDocuments => Set<CategorieDocument>();

        // Communication
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<UserConversation> UserConversations => Set<UserConversation>();

        // Locaux commerciaux
        public DbSet<LocalCommercial> LocauxCommerciaux { get; set; }
        public DbSet<ActiviteCommerciale> ActivitesCommerciales => Set<ActiviteCommerciale>();

        // Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<Prestataire> Prestataires { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= Résidences / Bâtiments / Lots =================
            modelBuilder.Entity<Residence>(b =>
            {
                b.Property(x => x.Nom).HasMaxLength(200).IsRequired();
                b.Property(x => x.Adresse).HasMaxLength(300);
                b.Property(x => x.Ville).HasMaxLength(150);
                b.Property(x => x.CodePostal).HasMaxLength(20);
            });

            modelBuilder.Entity<Batiment>().ToTable("Batiment");
            modelBuilder.Entity<Batiment>()
                .HasOne(b => b.Residence)
                .WithMany()
                .HasForeignKey(b => b.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Residence)
                .WithMany(r => r.Lots)
                .HasForeignKey(l => l.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocataireTemporaire>()
                .HasOne(lt => lt.Lot)
                .WithMany(l => l.LocationsTemporaires)
                .HasForeignKey(lt => lt.LotId)
                .OnDelete(DeleteBehavior.Cascade);

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

                b.Ignore("ApplicationUserId");
                b.Ignore("ApplicationUserId1");
                b.Ignore("ApplicationUserId2");
            });

            // ================= Messagerie =================
            modelBuilder.Entity<UserConversation>(b =>
            {
                b.HasKey(uc => new { uc.UserId, uc.ConversationId });

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(uc => uc.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(uc => uc.Conversation)
                 .WithMany(c => c.UserConversations)
                 .HasForeignKey(uc => uc.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(b =>
            {
                b.HasOne(m => m.Conversation)
                 .WithMany(c => c.Messages)
                 .HasForeignKey(m => m.ConversationId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(m => m.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= Assemblées =================
            modelBuilder.Entity<Vote>(b =>
            {
                b.HasOne(v => v.AssembleeGenerale)
                 .WithMany(ag => ag.Votes)
                 .HasForeignKey(v => v.AssembleeGeneraleId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(v => v.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(v => new { v.AssembleeGeneraleId, v.UserId })
                 .IsUnique()
                 .HasDatabaseName("UX_Vote_UniqueParAG");
            });

            modelBuilder.Entity<Convocation>(b =>
            {
                b.HasOne(c => c.AssembleeGenerale)
                 .WithMany(ag => ag.Convocations)
                 .HasForeignKey(c => c.AssembleeGeneraleId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<Notification>(b =>
            {
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
