using System;

namespace UserAuthProject.Models.User
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordEncrypted { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string Token { get; set; }
    }
}