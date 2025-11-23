using System;
using System.Collections.Generic;
using System.Linq;

namespace SyndicApp.Mobile.Security
{
    public static class UserRoleStore
    {
        private static List<string> _roles = new();

        public static IReadOnlyList<string> Roles => _roles;

        public static void SetRoles(IEnumerable<string>? roles)
        {
            if (roles == null)
            {
                _roles = new List<string>();
                return;
            }

            _roles = roles
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public static bool IsInRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return false;
            return _roles.Contains(role.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsInAnyRole(IEnumerable<string> roles)
        {
            var list = roles
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return _roles.Any(r => list.Contains(r, StringComparer.OrdinalIgnoreCase));
        }
    }
}
