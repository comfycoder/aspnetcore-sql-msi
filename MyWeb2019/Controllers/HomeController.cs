using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyWeb2019.Models;

namespace MyWeb2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly AdventureWorksContext _context;

        public HomeController(IConfiguration config, AdventureWorksContext context)
        {
            _config = config;
            _context = context;
        }

        public IActionResult Index()
        {
            var value = _config["MySecret"];

            var computerName = _config["COMPUTERNAME"];

            var customers = _context.Customers.Take(50).ToList();

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
