using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using UserAuthProject.Exceptions;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public IUserRepository UserRepository { get; set; }
        public IPasswordEncryptionService PasswordEncryptionService { get; set; }
        public Dictionary<Guid, string> SessionsKeys { get; set; } = new Dictionary<Guid, string>();

        private readonly string EMAIL_REGEX =
            "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

        public AuthenticationService(IUserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService)
        {
            this.UserRepository = userRepository;
            this.PasswordEncryptionService = passwordEncryptionService;
        }

        public async Task<string> Login(UserLoginData userLoginData)
        {
            var isEmail = Regex.IsMatch(userLoginData.UsernameOrEmail, EMAIL_REGEX);
            var user = isEmail
                ? await UserRepository.GetUserByEmail(userLoginData.UsernameOrEmail)
                : await UserRepository.GetUserByUsername(userLoginData.UsernameOrEmail);
            if (user == null)
            {
                return null;
            }

            return GenerateSessionKey(user);
        }

        public async Task Logout(Guid id)
        {
            var user = await UserRepository.GetUserById(id);
            if (user == null || !SessionsKeys.ContainsKey(id))
            {
                return;
            }

            SessionsKeys.Remove(id);
        }

        public async Task<string> Register(UserRegisterData userRegisterData)
        {
            if (!Regex.IsMatch(userRegisterData.Email, EMAIL_REGEX))
            {
                throw new RegisterException(userRegisterData, "Invalid email format");
            }

            if (await UserRepository.GetUserByUsername(userRegisterData.Username) != null)
            {
                throw new RegisterException(userRegisterData, "User by this username already exists");
            }
            if (await UserRepository.GetUserByEmail(userRegisterData.Email) != null)
            {
                throw new RegisterException(userRegisterData, "User by this email already exists");
            }

            var user = new User()
            {
                Username = userRegisterData.Username,
                Email = userRegisterData.Email,
                PasswordEncrypted = PasswordEncryptionService.EncryptPassword(userRegisterData.Password)
            };
            var registeredUser = await UserRepository.AddUser(user);
            return GenerateSessionKey(registeredUser);
        }

        public string GenerateSessionKey(User user)
        {
            var key = Guid.NewGuid().ToString();
            SessionsKeys.Add(user.Id, key);
            return key;
        }

        public bool IsValidSessionKey(string sessionKey) => SessionsKeys.ContainsValue(sessionKey);

        public async Task<User> GetUserBySessionKey(string sessionKey) =>
            await UserRepository.GetUserById(SessionsKeys.FirstOrDefault(pair => pair.Value.Equals(sessionKey)).Key);
    }
}