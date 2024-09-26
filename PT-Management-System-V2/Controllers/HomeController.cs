using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using System.Diagnostics;
using Npgsql;
//using PT_Management_System_V2.Helpers;

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
            //string username = Npgsql. 
            //UserHelper.ConnectDB();
            return View();
        }

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
