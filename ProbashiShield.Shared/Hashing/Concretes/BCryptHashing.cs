using ProbashiShield.Shared.Hashing.Contracts;

namespace ProbashiShield.Shared.Hashing.Concretes
{
    public class BCryptHashing : IHashManager
    {
        public string Hash(string PlainText)
        {
            return BCrypt.Net.BCrypt.HashPassword(PlainText);
        }

        public bool Verify(string PlainText, string HashedText)
        {
            return BCrypt.Net.BCrypt.Verify(PlainText, HashedText);
        }
    }
}
