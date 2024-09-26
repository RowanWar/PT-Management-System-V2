using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using PT_Management_System_V2.Services;

namespace PT_Management_System_V2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserLogin UserLogin)
        {
            SecurityService securityService = new SecurityService();
            if (securityService.IsValid(UserLogin))
            {
                return View("LoginSuccess", UserLogin);
            } 
            else
            {
                return View("LoginFailure", UserLogin);
            }
            
        }
    }
}
