﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Controllers
{
    public class LoginController : Controller
    {
        public static readonly string SessionKeyProperty = "SessionKey";
        public static readonly string UserIdProperty = "UserId";

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
            if (AuthenticationService.IsAuthenticated(HttpContextAccessor))
            {
                return RedirectToAction("Index", "ControlPanel");
            }
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(UserLoginData loginData)
        {
            if (AuthenticationService.IsAuthenticated(HttpContextAccessor))
            {
                return RedirectToAction("Index", "ControlPanel");
            }

            var userData = AuthenticationService.Login(loginData).Result;
            if (userData == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var token = userData.Token;
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home");
            }
            HttpContextAccessor.HttpContext.Response.Cookies.Append(UserIdProperty, userData.Id.ToString());
            HttpContextAccessor.HttpContext.Response.Cookies.Append(SessionKeyProperty, token);
            return RedirectToAction("Index", "ControlPanel");
        }

        public static string ReadCookie(IHttpContextAccessor httpContextAccessor, string cookie)
        {
            var listKeys = httpContextAccessor.HttpContext.Request.Cookies.Keys;
            string sesKey = null;
            foreach (var s in listKeys)
            {
                var read = httpContextAccessor.HttpContext.Request.Cookies[s];
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