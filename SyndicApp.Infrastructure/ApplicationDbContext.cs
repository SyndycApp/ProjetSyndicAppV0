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
        public DbSet<Intervention> Interventions => Set<Intervention>();

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

                b.HasOne<ApplicationUser>()      // relation sans nav
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .HasPrincipalKey(u => u.Id)
                 .HasConstraintName("FK_AffectationsLots_AspNetUsers_UserId")
                 .OnDelete(DeleteBehavior.Cascade);

                // neutralise toute prop fantôme potentielle
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
                b.HasOne(i => i.Lot)
                 .WithMany(l => l.Incidents)
                 .HasForeignKey(i => i.LotId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(i => i.Residence)
                 .WithMany(r => r.Incidents)      // <- mets r => r.Incidents si elle existe
                 .HasForeignKey(i => i.ResidenceId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(i => i.DeclareParId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Intervention>(b =>
            {
                b.HasOne(i => i.Incident)
                 .WithMany(x => x.Interventions)
                 .HasForeignKey(i => i.IncidentId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(i => i.Employe)
                 .WithMany(e => e.Interventions)
                 .HasForeignKey(i => i.EmployeId)
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
                 .WithMany(r => r.Documents)      // <- mets r => r.Documents si tu as cette collection
                 .HasForeignKey(d => d.ResidenceId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(d => d.AjouteParId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ================= Annonces =================
            modelBuilder.Entity<Annonce>(b =>
            {
                b.HasOne(a => a.Categorie)
                 .WithMany(c => c.Annonces)
                 .HasForeignKey(a => a.CategorieId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(a => a.Residence)
                 .WithMany(r => r.Annonces)       // <- mets r => r.Annonces si la prop existe, sinon WithMany()
                 .HasForeignKey(a => a.ResidenceId)
                 .OnDelete(DeleteBehavior.NoAction);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Locaux commerciaux
            modelBuilder.Entity<LocalCommercial>(b =>
            {
                b.ToTable("LocauxCommerciaux");

                // Propriétaire (AspNetUsers) – sans navigation
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(l => l.ProprietaireId)
                 .OnDelete(DeleteBehavior.NoAction);   // évite les cascades multiples

                // Locataire (AspNetUsers) – sans navigation
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(l => l.LocataireId)
                 .OnDelete(DeleteBehavior.NoAction);   // idem

                // Activité
                b.HasOne(l => l.Activite)
                 .WithMany()
                 .HasForeignKey(l => l.ActiviteId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Lot
                b.HasOne(l => l.Lot)
                 .WithMany()
                 .HasForeignKey(l => l.LotId)
                 .OnDelete(DeleteBehavior.Cascade);
            });


            // ================= Notifications =================
            modelBuilder.Entity<Notification>(b =>
            {
                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Auto-applique les IEntityTypeConfiguration<> si présentes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
