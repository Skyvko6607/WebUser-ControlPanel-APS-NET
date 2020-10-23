using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserAuthProject.Models.DbContexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User.User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
