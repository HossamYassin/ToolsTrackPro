namespace ToolsTrackPro.Application.Utilities
{
    using System;
    using System.Security.Cryptography;

    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 16-byte salt (128-bit)
        private const int HashSize = 32; // 32-byte hash (256-bit)
        private const int Iterations = 100000;

        /// <summary>
        /// Hashes a password using PBKDF2 with HMACSHA256.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt); // Generate a random salt

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    // Combine salt and hash into a single string (Base64 encoding)
                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        /// <summary>
        /// Verifies a password against a stored PBKDF2 hash.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Compare stored hash with computed hash
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false; // Password does not match
                    }
                }
            }

            return true; // Password matches
        }
    }

}
