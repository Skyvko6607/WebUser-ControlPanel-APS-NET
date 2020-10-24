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
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public AuthorizedController(IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.AuthenticationService = authenticationService;
            this.HttpContextAccessor = httpContextAccessor;
        }
    }
}