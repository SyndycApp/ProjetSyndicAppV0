using SyndicApp.Domain.Entities.Common;
using System.Collections.Generic;

namespace SyndicApp.Domain.Entities.Users;

public class Role : BaseEntity
{
    public string Nom { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}
