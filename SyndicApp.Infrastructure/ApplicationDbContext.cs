using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Domain.Entities.Annonces;
using SyndicApp.Domain.Entities.Assemblees;
using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Incidents;
using SyndicApp.Domain.Entities.LocauxCommerciaux;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Entities.Users;
using System;

namespace SyndicApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Users
        public DbSet<User> UsersApp => Set<User>();
        public DbSet<AffectationLot> AffectationsLots => Set<AffectationLot>();

        // Residences
        public DbSet<Residence> Residences => Set<Residence>();
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

        // Locaux commerciaux
        public DbSet<LocalCommercial> LocauxCommerciaux => Set<LocalCommercial>();
        public DbSet<ActiviteCommerciale> ActivitesCommerciales => Set<ActiviteCommerciale>();

        // Notifications
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relations AffectationLot
            modelBuilder.Entity<AffectationLot>()
                .HasKey(al => new { al.UserId, al.LotId });

            modelBuilder.Entity<AffectationLot>()
                .HasOne(al => al.User)
                .WithMany(u => u.AffectationsLots)
                .HasForeignKey(al => al.UserId);

            modelBuilder.Entity<AffectationLot>()
                .HasOne(al => al.Lot)
                .WithMany(l => l.AffectationsLots)
                .HasForeignKey(al => al.LotId);

            // Paiement
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.AppelDeFonds)
                .WithMany(a => a.Paiements)
                .HasForeignKey(p => p.AppelDeFondsId);

            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.User)
                .WithMany(u => u.Paiements)
                .HasForeignKey(p => p.UserId);

            // Intervention
            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Incident)
                .WithMany(x => x.Interventions)
                .HasForeignKey(i => i.IncidentId);

            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Employe)
                .WithMany(e => e.Interventions)
                .HasForeignKey(i => i.EmployeId);

            // Vote
            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.AssembleeGeneraleId, v.UserId });

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.AssembleeGenerale)
                .WithMany(ag => ag.Votes)
                .HasForeignKey(v => v.AssembleeGeneraleId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId);

            // Lot -> Residence
            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Residence)
                .WithMany(r => r.Lots)
                .HasForeignKey(l => l.ResidenceId);

            // LocataireTemporaire -> Lot
            modelBuilder.Entity<LocataireTemporaire>()
                .HasOne(lt => lt.Lot)
                .WithMany(l => l.LocationsTemporaires)
                .HasForeignKey(lt => lt.LotId);

            // Document -> Catégorie
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Categorie)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CategorieId);

            // Annonce -> Catégorie
            modelBuilder.Entity<Annonce>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Annonces)
                .HasForeignKey(a => a.CategorieId);

            // LocalCommercial -> Activite
            modelBuilder.Entity<LocalCommercial>()
                .HasOne(l => l.Activite)
                .WithMany(a => a.Locaux)
                .HasForeignKey(l => l.ActiviteId);

            // Message -> Conversation
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}