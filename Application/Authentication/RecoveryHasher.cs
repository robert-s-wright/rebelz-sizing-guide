using Application.Authentication.Interfaces;
using System.Security.Cryptography;

namespace Application.Authentication
{
    internal class RecoveryHasher : IRecoveryHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private static char Delimiter = ';';

        public string Hash(string email)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(email, salt, Iterations, _hashAlgorithmName, KeySize);



            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));

        }



        public bool Verify(string key, string hashedKey)
        {
            var elements = hashedKey.Split(Delimiter);
            //var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var inputElements = key.Split(Delimiter);
            var inputHash = Convert.FromBase64String(inputElements[1]);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
