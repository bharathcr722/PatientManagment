using Konscious.Security.Cryptography;
using System.Text;

namespace PatientManagment.Models.Encryption
{
    public static class Argon2Utils
    {
        private const int SaltSize = 16; 
        private const int HashSize = 32; 
        private const int Iterations = 4;
        private const int MemorySize = 65536; 
        private const int DegreeOfParallelism = 8; 

        private static byte[] GenerateSalt()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
        }
        private static string GetHashPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var hasher = new Argon2id(passwordBytes))
            {
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = DegreeOfParallelism;
                hasher.MemorySize = MemorySize;
                hasher.Iterations = Iterations;

                var hashBytes = hasher.GetBytes(HashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static (string hashPassword, byte[] salt) HashPassword(string password)
        {
            var salt = GenerateSalt();
            return (GetHashPassword(password, salt), salt);

        }

        public static bool VerifyPassword(string password, string storedHash, byte[] salt)
        {
            var hash = GetHashPassword(password, salt);
            return hash.Equals(storedHash);
        }
    }
}
