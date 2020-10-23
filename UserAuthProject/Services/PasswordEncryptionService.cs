using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Services
{
    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        public string EncryptPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}