using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.Models.User;

namespace UserAuthProject.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(UserLoginData userLoginData);
        Task Logout(Guid id);
        Task<string> Register(UserRegisterData userRegisterData);
        string GenerateSessionKey(User user);
        bool IsValidSessionKey(string sessionKey);
        Task<User> GetUserBySessionKey(string sessionKey);
    }
}
