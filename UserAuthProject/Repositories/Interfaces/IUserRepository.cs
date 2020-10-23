using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.Models.User;

namespace UserAuthProject.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task RemoveUser(User user);
    }
}
