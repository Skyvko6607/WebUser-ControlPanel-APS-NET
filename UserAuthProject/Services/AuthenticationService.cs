using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
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

            var token = GenerateSessionKey(user.Id);
            user.Token = token;
            return await UserRepository.UpdateUser(user);
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
            var id = new Guid();
            var token = GenerateSessionKey(id);
            var user = new User()
            {
                Id = id,
                Username = userRegisterData.Username,
                Email = userRegisterData.Email,
                PasswordSalt = salt,
                PasswordEncrypted = PasswordEncryptionService.CreateHash(userRegisterData.Password, salt),
                Token = token
            };
            var registeredUser = await UserRepository.AddUser(user);
            return registeredUser;
        }

        public string GenerateSessionKey(Guid id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("7W6FueCxVULp0tBDR4QD");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
    }
}