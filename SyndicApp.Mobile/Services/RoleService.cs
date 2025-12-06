using Microsoft.Maui.Storage;

namespace SyndicApp.Mobile.Services
{
    public class RoleService
    {
        private readonly string _role;

        public RoleService()
        {
            _role = Preferences.Get("user_role", "").ToLowerInvariant();
        }

        public bool IsSyndic => _role.Contains("syndic");
        public bool IsCopro => _role.Contains("copro");
        public bool IsGardien => _role.Contains("gardien");

        public bool HasRole(string roleName)
            => _role.Contains(roleName.ToLowerInvariant());

        public string GetRawRole() => _role;
    }
}
