using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;
using IAuthenticationService = UserAuthProject.Services.Interfaces.IAuthenticationService;

namespace UserAuthProject.Controllers
{
    public class LoginController : Controller
    {
        public static readonly string SessionKeyProperty = "SessionKey";

        public IUserRepository UserRepository { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public LoginController(IUserRepository userRepository, IAuthenticationService authenticationService,
            IHttpContextAccessor httpContext)
        {
            this.UserRepository = userRepository;
            this.AuthenticationService = authenticationService;
            this.HttpContextAccessor = httpContext;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "ControlPanel");
            }

            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(UserLoginData loginData)
        {
            if (!string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "ControlPanel");
            }

            var userData = AuthenticationService.Login(loginData).Result;
            if (userData == null)
            {
                return RedirectToAction("Index", "Login");
            }

            HttpContextAccessor.HttpContext.Response.Cookies.Append(SessionKeyProperty, userData.Token);

            return RedirectToAction("Index", "ControlPanel");
        }

        public IActionResult Logout()
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Home");
            }

            Guid id = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            AuthenticationService.Logout(id);
            HttpContextAccessor.HttpContext.Response.Cookies.Delete(SessionKeyProperty);

            return RedirectToAction("Index", "Home");
        }
    }
}