using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using UserAuthProject.Controllers;
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

        private readonly string EMAIL_REGEX =
            "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

        public AuthenticationService(IUserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService)
        {
            this.UserRepository = userRepository;
            this.PasswordEncryptionService = passwordEncryptionService;
        }

        public async Task<User> Login(UserLoginData userLoginData)
        {
            var isEmail = Regex.IsMatch(userLoginData.UsernameOrEmail, EMAIL_REGEX);
            var user = isEmail
                ? await UserRepository.GetUserByEmail(userLoginData.UsernameOrEmail)
                : await UserRepository.GetUserByUsername(userLoginData.UsernameOrEmail);
            if (user == null)
            {
                return null;
            }

            var passwordEncrypted = PasswordEncryptionService.CreateHash(userLoginData.Password, user.PasswordSalt);
            if (!user.PasswordEncrypted.Equals(passwordEncrypted))
            {
                return null;
            }

            return await GenerateSessionKey(user);
        }

        public async Task Logout(Guid id)
        {
            var user = await UserRepository.GetUserById(id);
            if (user == null || string.IsNullOrEmpty(user.Token))
            {
                return;
            }

            user.Token = null;
            await UserRepository.UpdateUser(user);
        }

        public async Task<User> Register(UserRegisterData userRegisterData)
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

            var salt = PasswordEncryptionService.CreateSalt();
            var user = new User()
            {
                Username = userRegisterData.Username,
                Email = userRegisterData.Email,
                PasswordSalt = salt,
                PasswordEncrypted = PasswordEncryptionService.CreateHash(userRegisterData.Password, salt)
            };
            var registeredUser = await UserRepository.AddUser(user);
            return await GenerateSessionKey(registeredUser);
        }

        public async Task<User> GenerateSessionKey(User user)
        {
            var key = Guid.NewGuid().ToString();
            user.Token = key;
            var newUser = await UserRepository.UpdateUser(user);
            return newUser;
        }

        public async Task<bool> IsValidSessionKey(Guid id, string sessionKey)
        {
            var user = await UserRepository.GetUserById(id);
            if (user == null || string.IsNullOrEmpty(sessionKey) || string.IsNullOrEmpty(user.Token))
            {
                return await new Task<bool>(() => false);
            }
            return user.Token.Equals(sessionKey);
        }

        public bool IsAuthenticated(IHttpContextAccessor httpContextAccessor)
        {
            string sesKey = LoginController.ReadCookie(httpContextAccessor, LoginController.SessionKeyProperty);
            string userKey = LoginController.ReadCookie(httpContextAccessor, LoginController.UserIdProperty);
            if (!string.IsNullOrEmpty(sesKey) && !string.IsNullOrEmpty(userKey))
            {
                if (IsValidSessionKey(Guid.Parse(userKey), sesKey).Result)
                {
                    return true;
                }
            }

            return false;
        }
    }
}