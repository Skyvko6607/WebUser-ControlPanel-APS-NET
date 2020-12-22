using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (!string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "ControlPanel");
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(UserRegisterData registerData)
        {
            if (!string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "ControlPanel");
            }

            var userData = AuthenticationService.Register(registerData).Result;
            if (userData == null)
            {
                return RedirectToAction("Index", "Register");
            }
            return RedirectToAction("Index", "ControlPanel");
        }
    }
}