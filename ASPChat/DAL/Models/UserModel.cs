namespace ASPChat.DAL.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage {  get; set; }
        public int? Status { get; set; }
    }
}
