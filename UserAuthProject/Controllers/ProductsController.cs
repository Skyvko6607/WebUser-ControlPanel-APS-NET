using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAuthProject.Repositories.Interfaces;

namespace UserAuthProject.Controllers
{
    public class ProductsController : Controller
    {
        public IProductsRepository ProductsRepository { get; set; }

        public ProductsController(IProductsRepository productsRepository)
        {
            ProductsRepository = productsRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(ProductsRepository.GetProducts().Result);
        }
    }
}
