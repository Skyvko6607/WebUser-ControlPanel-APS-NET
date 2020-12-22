using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.Models.Webshop;

namespace UserAuthProject.Models.DbContexts
{
    public class GlobalDbContext : DbContext
    {
        public DbSet<User.User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReviewData> Reviews { get; set; }
        public DbSet<QuestionData> Questions { get; set; }
        public DbSet<AnswerData> Answers { get; set; }

        public GlobalDbContext(DbContextOptions<GlobalDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
