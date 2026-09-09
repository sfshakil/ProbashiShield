namespace ProbashiShield.Shared.Hashing.Contracts
{
    public interface IHashManager
    {
        string Hash(string PlainText);
        bool Verify(string PlainText, string HashedText);
    }
}
