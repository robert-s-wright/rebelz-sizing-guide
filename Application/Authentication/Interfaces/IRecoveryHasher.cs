namespace Application.Authentication.Interfaces
{
    internal interface IRecoveryHasher
    {
        string Hash(string email);

        bool Verify(string key, string hashedKey);
    }
}
