using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.Models.DbContexts;
using UserAuthProject.Models.Webshop;
using UserAuthProject.Repositories.Interfaces;

namespace UserAuthProject.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private GlobalDbContext GlobalDbContext { get; set; }

        public ProductsRepository(GlobalDbContext globalDbContext)
        {
            GlobalDbContext = globalDbContext;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await GlobalDbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            return await GlobalDbContext.Products.Where(product => product.Id == productId).FirstOrDefaultAsync();
        }
    }
}
