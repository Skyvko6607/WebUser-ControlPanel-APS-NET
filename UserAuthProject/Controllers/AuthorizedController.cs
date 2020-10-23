using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Controllers
{
    public class AuthorizedController : Controller
    {
        public IAuthenticationService AuthenticationService { get; set; }

        public AuthorizedController(IAuthenticationService authenticationService)
        {
            this.AuthenticationService = authenticationService;
        }

        public bool IsAuthenticated()
        {
            if (HttpContext.Session.GetString("SessionKey") != null)
            {
                if (AuthenticationService.IsValidSessionKey(HttpContext.Session.GetString("SessionKey")))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
