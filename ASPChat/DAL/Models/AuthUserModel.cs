﻿namespace ASPChat.DAL.Models
{
    public class AuthUserModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
