using ASPChat.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace ASPChat.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Wrong Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public UserModel ToUserMode()
        {
            return new UserModel
            {
                Email = this.Email,
                Password = this.Password,
                FirstName = this.FirstName,
                LastName = this.LastName
            };

        }
    }
}
