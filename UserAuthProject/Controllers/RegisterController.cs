using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Controllers
{
    public class RegisterController : Controller
    {
        public IUserRepository UserRepository { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public RegisterController(IUserRepository userRepository, IAuthenticationService authenticationService,
            IHttpContextAccessor httpContext)
        {
            this.UserRepository = userRepository;
            this.AuthenticationService = authenticationService;
            this.HttpContextAccessor = httpContext;
        }

        public IActionResult Index()
        {
            if (AuthenticationService.IsAuthenticated(HttpContextAccessor))
            {
                return RedirectToAction("Index", "ControlPanel");
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(UserRegisterData registerData)
        {
            if (AuthenticationService.IsAuthenticated(HttpContextAccessor))
            {
                return RedirectToAction("Index", "ControlPanel");
            }

            var userData = AuthenticationService.Register(registerData).Result;
            if (userData == null)
            {
                return RedirectToAction("Index", "Register");
            }
            var token = userData.Token;
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home");
            }
            HttpContextAccessor.HttpContext.Response.Cookies.Append(LoginController.UserIdProperty, userData.Id.ToString());
            HttpContextAccessor.HttpContext.Response.Cookies.Append(LoginController.SessionKeyProperty, token);
            return RedirectToAction("Index", "ControlPanel");
        }
    }
}