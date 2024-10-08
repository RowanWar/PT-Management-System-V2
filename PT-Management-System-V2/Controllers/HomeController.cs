using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using System.Diagnostics;
using Npgsql;
using Microsoft.AspNetCore.Authorization;


namespace PT_Management_System_V2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();

        }

        [Authorize]
        public IActionResult Privacy()
        {
            ViewBag.Message = "Security is important";
            ViewBag.MyFavouriteColor = "Green";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
