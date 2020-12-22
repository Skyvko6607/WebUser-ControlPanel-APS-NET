using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserAuthProject.Models;
using UserAuthProject.Repositories.Interfaces;

namespace UserAuthProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IReviewDataRepository ReviewDataRepository { get; set; }

        public HomeController(ILogger<HomeController> logger, IReviewDataRepository reviewDataRepository)
        {
            _logger = logger;
            ReviewDataRepository = reviewDataRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Reviews(Guid productId)
        {
            return View(ReviewDataRepository.GetProductReview(productId).Result);
        }
    }
}
