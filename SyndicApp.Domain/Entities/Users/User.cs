using SyndicApp.Domain.Entities.Common;
using SyndicApp.Domain.Entities.Communication;
using SyndicApp.Domain.Entities.Finances;
using SyndicApp.Domain.Entities.Residences;
using SyndicApp.Domain.Entities.Assemblees;
using System;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Users;

public class User : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;

    // Relations
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<AffectationLot> Affectations { get; set; } = new List<AffectationLot>();
    public ICollection<Message> MessagesEnvoyes { get; set; } = new List<Message>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
    public ICollection<AffectationLot> AffectationsLots { get; set; } = new List<AffectationLot>();
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();


}
