﻿using System;
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
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 24;
        private const int ITERATIONS = 100000;

        public string CreateHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException();
            }

            string resultHash = null;

            try
            {
                byte[] saltArray = Convert.FromBase64String(salt.ToLower());
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltArray, ITERATIONS);
                byte[] hashBytes = pbkdf2.GetBytes(HASH_SIZE);

                resultHash = Convert.ToBase64String(hashBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultHash;
        }

        public string CreateSalt()
        {
            string resultSalt = null;

            try
            {
                RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                byte[] rawSalt = new byte[SALT_SIZE];
                provider.GetBytes(rawSalt);

                resultSalt = Convert.ToBase64String(rawSalt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultSalt?.ToLower();
        }
    }
}