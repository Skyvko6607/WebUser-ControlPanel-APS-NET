using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthProject.Models.Webshop;

namespace UserAuthProject.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(Guid productId);
    }
}
