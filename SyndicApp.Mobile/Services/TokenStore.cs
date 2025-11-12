// Services/TokenStore.cs
using System;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace SyndicApp.Mobile.Services
{
    public class TokenStore
    {
        private const string TokenKey = "auth_token";
        private const string RoleKey = "user_role";
        private const string UserIdKey = "user_id";

        // ====== TOKEN ======
        public void SaveToken(string token) => Preferences.Set(TokenKey, token);
        public string? GetToken() => Preferences.Get(TokenKey, null);
        public void ClearToken() => Preferences.Remove(TokenKey);

        // ====== ROLE ======
        public void SaveRole(string role) => Preferences.Set(RoleKey, role);
        public string? GetRole() => Preferences.Get(RoleKey, null);
        public bool IsSyndic() => string.Equals(GetRole(), "Syndic", StringComparison.OrdinalIgnoreCase);

        // ====== USER ID ======
        public void SaveUserId(string userId) => Preferences.Set(UserIdKey, userId);
        public string? GetSavedUserId() => Preferences.Get(UserIdKey, null);

        /// <summary>
        /// Retourne l'UserId :
        /// 1) depuis les préférences si déjà sauvegardé
        /// 2) sinon, tente de le lire depuis le JWT (claims: sub, nameid, uid)
        /// </summary>
        public string? GetUserId()
        {
            var cached = GetSavedUserId();
            if (!string.IsNullOrWhiteSpace(cached))
                return cached;

            var token = GetToken();
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var fromJwt = TryGetUserIdFromJwt(token);
            if (!string.IsNullOrWhiteSpace(fromJwt))
            {
                SaveUserId(fromJwt);
                return fromJwt;
            }
            return null;
        }

        public void Clear()
        {
            SecureStorage.Remove(TokenKey);
            Preferences.Remove(TokenKey);
            Preferences.Remove(RoleKey);
            Preferences.Remove(UserIdKey);
        }

        // ====== Helpers ======
        private static string? TryGetUserIdFromJwt(string jwt)
        {
            try
            {
                // JWT = "header.payload.signature"
                var parts = jwt.Split('.');
                if (parts.Length < 2) return null;

                var payloadJson = Base64UrlDecode(parts[1]);
                if (string.IsNullOrWhiteSpace(payloadJson)) return null;

                using var doc = JsonDocument.Parse(payloadJson);
                var root = doc.RootElement;

                // Essaye les claims classiques
                if (root.TryGetProperty("sub", out var sub) && sub.ValueKind == JsonValueKind.String)
                    return sub.GetString();

                if (root.TryGetProperty("nameid", out var nameid) && nameid.ValueKind == JsonValueKind.String)
                    return nameid.GetString();

                if (root.TryGetProperty("uid", out var uid) && uid.ValueKind == JsonValueKind.String)
                    return uid.GetString();

                // Parfois les claims sont préfixés (ex: "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                foreach (var prop in root.EnumerateObject())
                {
                    var key = prop.Name.ToLowerInvariant();
                    if ((key.Contains("nameidentifier") || key.EndsWith("/nameidentifier") || key == "userid" || key.EndsWith("/nameidentifier"))
                        && prop.Value.ValueKind == JsonValueKind.String)
                    {
                        return prop.Value.GetString();
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string Base64UrlDecode(string input)
        {
            // Remplacer '-' par '+' et '_' par '/'
            string output = input.Replace('-', '+').Replace('_', '/');
            // Ajouter le padding
            switch (output.Length % 4)
            {
                case 2: output += "=="; break;
                case 3: output += "="; break;
            }
            var bytes = Convert.FromBase64String(output);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
