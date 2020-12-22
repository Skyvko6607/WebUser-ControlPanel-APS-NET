using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.Models.DbContexts;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;
using UserAuthProject.Services.Interfaces;

namespace UserAuthProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : AuthorizedController
    {
        private GlobalDbContext db { get; set; }
        public IUserRepository UserRepository { get; set; }

        public UsersController(GlobalDbContext userDbContext, IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor, IAuthenticationService authenticationService) : base(
            authenticationService, httpContextAccessor)
        {
            this.db = userDbContext;
            this.UserRepository = userRepository;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Route("details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return BadRequest();
            }

            User user = await UserRepository.GetUserById(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Route("edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return BadRequest();
            }

            User user = await UserRepository.GetUserById(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(include: "Username,Email,RegistrationDate")]
            User user)
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Delete/5
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return BadRequest();
            }

            User user = await UserRepository.GetUserById(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.Name)))
            {
                return RedirectToAction("Index", "Login");
            }

            User user = await UserRepository.GetUserById(Guid.Parse(id));
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}