using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.LocauxCommerciaux;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Infrastructure.Identity;
using SyndicApp.Domain.Entities.Communication;
using System;

namespace SyndicApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Affectations
        public DbSet<AffectationLot> AffectationsLots => Set<AffectationLot>();

        // Résidences
        public DbSet<Residence> Residences => Set<Residence>();
        public DbSet<Batiment> Batiments => Set<Batiment>();
        public DbSet<Lot> Lots => Set<Lot>();
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
        public DbSet<LocalCommercial> LocauxCommerciaux => Set<LocalCommercial>();
        public DbSet<ActiviteCommerciale> ActivitesCommerciales => Set<ActiviteCommerciale>();

        // Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Residence
            modelBuilder.Entity<Residence>(b =>
            {
                b.Property(x => x.Nom).HasMaxLength(200).IsRequired();
                b.Property(x => x.Adresse).HasMaxLength(300);
                b.Property(x => x.Ville).HasMaxLength(150);
                b.Property(x => x.CodePostal).HasMaxLength(20);
            });

            // Lot -> Residence
            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Residence)
                .WithMany(r => r.Lots)
                .HasForeignKey(l => l.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Batiment (table au singulier)
            modelBuilder.Entity<Batiment>().ToTable("Batiment");

            modelBuilder.Entity<Batiment>()
                .HasOne(b => b.Residence)
                .WithMany()
                .HasForeignKey(b => b.ResidenceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pas de FK Lot -> Batiment
            modelBuilder.Entity<Lot>()
                .Ignore(l => l.AffectationsLots);

            // LocataireTemporaire -> Lot
            modelBuilder.Entity<LocataireTemporaire>()
                .HasOne(lt => lt.Lot)
                .WithMany(l => l.LocationsTemporaires)
                .HasForeignKey(lt => lt.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            // AffectationLot
            modelBuilder.Entity<AffectationLot>(b =>
            {
                b.HasOne(a => a.Lot)
                 .WithMany(l => l.Affectations)
                 .HasForeignKey(a => a.LotId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ApplicationUser>()
                 .WithMany()
                 .HasForeignKey(a => a.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.Property(a => a.EstProprietaire).IsRequired();

                b.HasIndex(a => new { a.LotId, a.UserId, a.DateFin })
                 .HasDatabaseName("IX_AffectationLot_UniqueActive");
            });

            // ApplicationDbContext.OnModelCreating
            modelBuilder.Entity<AffectationLot>()
                .HasIndex(a => new { a.LotId })
                .HasDatabaseName("UX_Lot_OccupantActif")
                .HasFilter("[DateFin] IS NULL")
                .IsUnique();



            // Messagerie
            modelBuilder.Entity<UserConversation>()
                .HasKey(uc => new { uc.UserId, uc.ConversationId });

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConversations)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            // Assemblées
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.AssembleeGenerale)
                .WithMany(ag => ag.Votes)
                .HasForeignKey(v => v.AssembleeGeneraleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.AssembleeGeneraleId, v.UserId })
                .IsUnique()
                .HasDatabaseName("UX_Vote_UniqueParAG");

            // Finances
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.AppelDeFonds)
                .WithMany(a => a.Paiements)
                .HasForeignKey(p => p.AppelDeFondsId);

            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.User)
                .WithMany(u => u.Paiements)
                .HasForeignKey(p => p.UserId);

            // Incidents
            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Incident)
                .WithMany(x => x.Interventions)
                .HasForeignKey(i => i.IncidentId);

            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Employe)
                .WithMany(e => e.Interventions)
                .HasForeignKey(i => i.EmployeId);

            // Documents
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Categorie)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CategorieId);

            // Annonces
            modelBuilder.Entity<Annonce>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Annonces)
                .HasForeignKey(a => a.CategorieId);

            // Locaux commerciaux
            modelBuilder.Entity<LocalCommercial>()
                .HasOne(l => l.Activite)
                .WithMany(a => a.Locaux)
                .HasForeignKey(l => l.ActiviteId);

            // Decimals
            modelBuilder.Entity<AppelDeFonds>()
                .Property(a => a.MontantTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Charge>()
                .Property(c => c.Montant)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Paiement>()
                .Property(p => p.Montant)
                .HasPrecision(18, 2);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
