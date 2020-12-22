using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using UserAuthProject.Models.User;

namespace UserAuthProject.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Login(UserLoginData userLoginData);
        Task Logout(Guid id);
        Task<User> Register(UserRegisterData userRegisterData);
        string GenerateSessionKey(Guid id);
        Task<bool> IsValidSessionKey(Guid id, string sessionKey);

        public static string ReadCookie(MessageReceivedContext httpContextAccessor, string cookie)
        {
            return ReadCookie(httpContextAccessor.Request.Cookies, cookie);
        }
        public static string ReadCookie(IHttpContextAccessor httpContextAccessor, string cookie)
        {
            return ReadCookie(httpContextAccessor.HttpContext.Request.Cookies, cookie);
        }
        public static string ReadCookie(IRequestCookieCollection collection, string cookie)
        {
            var listKeys = collection.Keys;
            string sesKey = null;
            foreach (var s in listKeys)
            {
                var read = collection[s];
                if (s.Equals(cookie))
                {
                    sesKey = read;
                    break;
                }
            }

            return sesKey;
        }
    }
}
