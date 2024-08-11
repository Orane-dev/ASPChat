using ASPChat.DAL.Models;
using ASPChat.Controllers;
using ASPChat.BuisnessLayer.Auth.Interfaces;
using System.ComponentModel.DataAnnotations;
using ASPChat.DAL.Interfaces;

namespace ASPChat.BuisnessLayer.Auth.Implementations
{
    public class Authentication : IAuthentication
    {
        private IAuthenticationDAL _authenticationDal;
        private IHttpContextAccessor _contextAccessor;
        private IEncrypt _encrypt;
        public Authentication(IAuthenticationDAL authenticationDal, IHttpContextAccessor httpContextAccessor, IEncrypt encrypt) 
        { 
            _encrypt = encrypt;
            _contextAccessor = httpContextAccessor;
            _authenticationDal = authenticationDal;
        }

        public async Task<int> RegisterUser(UserModel user)
        {
            user.Salt = Guid.NewGuid().ToString();
            user.Password = _encrypt.GetPasswordHash(user.Password, user.Salt);

            int id = await _authenticationDal.CreateUserAsync(user);

            this.Login(id);

            return id;
        }

        public async Task<bool> AuthenticateUser(string email, string password)
        {
            var user = await _authenticationDal.GetUserAsync(email);
            if (user.UserId == null)
            {
                return false;
            }
            if(user.Password == _encrypt.GetPasswordHash(password, user.Salt))
            {
                this.Login(user.UserId.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Login(int id)
        {
            _contextAccessor?.HttpContext?.Session.SetInt32("userId", id);

            var userIdCookie = _contextAccessor?.HttpContext?.Request.Cookies.ContainsKey("user_id");
            if (userIdCookie != null) 
            {
                if (!(bool)userIdCookie)
                    _contextAccessor?.HttpContext?.Response.Cookies.Append("user_id", id.ToString());
                
            }
            
        }

        public async Task<ValidationResult?> ValidateUserEmail(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                var user = await _authenticationDal.GetUserAsync(email);
                if (user.UserId != null)
                {
                    return new ValidationResult("Email already exist");
                }
            }
            
            return null;
        }
    }
}
