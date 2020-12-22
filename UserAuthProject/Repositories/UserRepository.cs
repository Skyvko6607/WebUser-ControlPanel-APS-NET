using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.Models.DbContexts;
using UserAuthProject.Models.User;
using UserAuthProject.Repositories.Interfaces;

namespace UserAuthProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private GlobalDbContext GlobalDbContext { get; set; }

        public UserRepository(GlobalDbContext globalDbContext)
        {
            this.GlobalDbContext = globalDbContext;
        }

        public async Task<User> GetUserById(Guid id) =>
            await GlobalDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetUserByUsername(string username) =>
            await GlobalDbContext.Users.Where(user =>
                user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

        public async Task<User> GetUserByEmail(string email) =>
            await GlobalDbContext.Users.Where(user =>
                user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

        public async Task<User> AddUser(User user)
        {
            if (await GetUserById(user.Id) != null)
            {
                return null;
            }

            var newUser = await GlobalDbContext.Users.AddAsync(user);
            await GlobalDbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<User> UpdateUser(User user)
        {
            if (await GetUserById(user.Id) == null)
            {
                return null;
            }

            var newUser = GlobalDbContext.Users.Update(user);
            await GlobalDbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task RemoveUser(User user)
        {
            if (await GetUserById(user.Id) == null)
            {
                return;
            }

            GlobalDbContext.Users.Remove(user);
            await GlobalDbContext.SaveChangesAsync();
        }
    }
}