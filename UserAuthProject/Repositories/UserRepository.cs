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
        public UserDbContext UserDbContext { get; set; }

        public UserRepository(UserDbContext userDbContext)
        {
            this.UserDbContext = userDbContext;
        }

        public async Task<User> GetUserById(Guid id) =>
            await UserDbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetUserByUsername(string username) =>
            await UserDbContext.Users.Where(user =>
                user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

        public async Task<User> GetUserByEmail(string email) =>
            await UserDbContext.Users.Where(user =>
                user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

        public async Task<User> AddUser(User user)
        {
            if (await GetUserById(user.Id) != null)
            {
                return null;
            }

            var newUser = await UserDbContext.Users.AddAsync(user);
            await UserDbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<User> UpdateUser(User user)
        {
            if (await GetUserById(user.Id) == null)
            {
                return null;
            }

            var newUser = UserDbContext.Users.Update(user);
            await UserDbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task RemoveUser(User user)
        {
            if (await GetUserById(user.Id) == null)
            {
                return;
            }

            UserDbContext.Users.Remove(user);
            await UserDbContext.SaveChangesAsync();
        }
    }
}