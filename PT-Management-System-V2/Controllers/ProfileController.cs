//using Microsoft.AspNetCore.Mvc;
//using PT_Management_System_V2.Models;
//using PT_Management_System_V2.Services;

//namespace PT_Management_System_V2.Controllers
//{
//    public class ProfileController : Controller
//    {
//        static List<CoachModel> profile = new List<CoachModel>();
//        public IActionResult Index(int CoachId)
//        {
//            ProfileDAO profile = new ProfileDAO();
//            List<CoachModel> coachProfile = profile.GetCoachProfileById(CoachId);
//            return View(coachProfile);
//        }
//    }
//}

