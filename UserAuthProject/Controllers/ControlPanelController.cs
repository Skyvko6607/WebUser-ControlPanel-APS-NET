using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthProject.Repositories.Interfaces;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ControlPanelController : AuthorizedController
    {
        public IUserRepository UserRepository { get; set; }

        public ControlPanelController(IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : base(authenticationService,
            httpContextAccessor)
        {
            this.UserRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var user = UserRepository.GetUserById(Guid.Parse(userId));

            return View(user.Result);
        }
    }
}