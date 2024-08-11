using ASPChat.DAL.Models;

namespace ASPChat.DAL.Interfaces
{
    public interface IAuthenticationDAL
    {
        Task<UserModel> GetUserAsync(string email);
        Task<UserModel> GetUserAsync(int userId);
        Task<int> CreateUserAsync(UserModel user);
    }
}
