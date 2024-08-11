using System.Security.Cryptography;
using System.Text;
using ASPChat.BuisnessLayer.Auth.Interfaces;

namespace ASPChat.BuisnessLayer.Auth.Implementations
{
    public class Encrypt : IEncrypt
    {
        public string GetPasswordHash(string password, string salt)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(salt+password);
            byte[] hashPassword = sHA256.ComputeHash(inputBytes);

            return Convert.ToHexString(hashPassword);
        }
    }
}
