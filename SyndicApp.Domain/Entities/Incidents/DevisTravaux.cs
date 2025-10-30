using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Documents;
using SyndicApp.Domain.Entities.Residences;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SyndicApp.Domain.Entities.Incidents.Enums;

namespace SyndicApp.Domain.Entities.Incidents
{
	public class DevisTravaux : BaseEntity
	{
		
		public string Titre { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public DateTime DateEmission { get; set; } = DateTime.UtcNow;

		public StatutDevis Statut { get; set; } = StatutDevis.EnAttente;

		public decimal MontantHT { get; set; }


		public decimal TauxTVA { get; set; } = 0.20m;

		[NotMapped]
		public decimal MontantTTC => Math.Round(MontantHT * (1 + TauxTVA), 2);


		public Guid ResidenceId { get; set; }
		public Residence Residence { get; set; } = null!;

		public Guid? IncidentId { get; set; }
		public Incident? Incident { get; set; }

		public ICollection<Document> Documents { get; set; } = new List<Document>();


		public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
		public ICollection<DevisHistorique> Historique { get; set; } = new List<DevisHistorique>();

		public Guid? ValideParId { get; set; }    // Syndic
		public DateTime? DateDecision { get; set; }
		public string? CommentaireDecision { get; set; }
	}
}
