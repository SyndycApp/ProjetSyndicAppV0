using System.Security.Cryptography;
using System.Text;

namespace SyndicApp.Infrastructure.Services.Common
{
    public static class HashService
    {
        public static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
