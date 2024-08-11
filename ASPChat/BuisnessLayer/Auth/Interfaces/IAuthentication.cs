using ASPChat.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace ASPChat.BuisnessLayer.Auth.Interfaces
{
    public interface IAuthentication
    {
        Task<int> RegisterUser(UserModel user);
        Task<bool> AuthenticateUser(string email, string password);
        void Login(int id);
        Task<ValidationResult?> ValidateUserEmail(string email);
    }
}
