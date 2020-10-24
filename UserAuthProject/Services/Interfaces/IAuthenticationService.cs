using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserAuthProject.Models.User;

namespace UserAuthProject.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Login(UserLoginData userLoginData);
        Task Logout(Guid id);
        Task<User> Register(UserRegisterData userRegisterData);
        Task<User> GenerateSessionKey(User user);
        Task<bool> IsValidSessionKey(Guid id, string sessionKey);
        bool IsAuthenticated(IHttpContextAccessor httpContextAccessor);
    }
}
