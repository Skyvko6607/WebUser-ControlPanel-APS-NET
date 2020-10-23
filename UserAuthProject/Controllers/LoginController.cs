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
    public class LoginController : Controller
    {
        public IUserRepository UserRepository { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }

        public LoginController(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            this.UserRepository = userRepository;
            this.AuthenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginData loginData)
        {
            if (HttpContext.Session.GetString("SessionKey") != null)
            {
                if (AuthenticationService.IsValidSessionKey(HttpContext.Session.GetString("SessionKey")))
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            var sessionKey = AuthenticationService.Login(loginData);
            if (string.IsNullOrEmpty(sessionKey.Result))
            {
                return RedirectToAction("Error", "Home");
            }
            HttpContext.Session.SetString("SessionKey", sessionKey.Result);
            return RedirectToAction("Index", "ControlPanel");
        }
    }
}