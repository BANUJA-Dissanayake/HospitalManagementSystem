using System;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Data
{
    // Simple one-way hashing so plaintext passwords are never stored or compared directly.
    public static class PasswordHasher
    {
        public static string Hash(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool Verify(string plainText, string hash) => Hash(plainText) == hash;
    }
}
