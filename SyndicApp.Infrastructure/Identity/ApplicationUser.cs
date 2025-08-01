using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using SyndicApp.Domain.Entities.Users;

namespace SyndicApp.Infrastructure.Identity
{

	public class ApplicationUser : IdentityUser<Guid>
	{

		public string FullName { get; set; } = string.Empty;
		public string? Adresse { get; set; }
		public DateTime? DateNaissance { get; set; }
		public ICollection<AffectationLot> AffectationsLots { get; set; } = new List<AffectationLot>();

		public DateTime DateCreation { get; set; } = DateTime.UtcNow;
		public bool IsActive { get; set; } = true;
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
