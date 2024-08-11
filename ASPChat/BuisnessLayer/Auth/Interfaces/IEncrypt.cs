namespace ASPChat.BuisnessLayer.Auth.Interfaces
{
    public interface IEncrypt
    {
        string GetPasswordHash(string password, string salt);
    }
}
