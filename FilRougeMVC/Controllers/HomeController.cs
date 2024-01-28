using FilRougeMVC.Data;
using FilRougeMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FilRougeMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        BibliothequeDbContext Context { get; }

        public HomeController(ILogger<HomeController> logger,
            BibliothequeDbContext context)
        {
            _logger = logger;
            Context = context;
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
    }
}
