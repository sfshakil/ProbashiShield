namespace ProbashiShield.Shared.Hashing.Contracts
{
    public interface ICipher
    {
        string EncryptString(string val);
        string DecryptString(string val);
    }
}
