using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Services;
using System.Security.Claims;

namespace PT_Management_System_V2.Controllers
{
    public class YourCoachController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;
        private readonly YourCoachDAO _yourcoachDAO;

        public YourCoachController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
            _clientDAO = clientDAO;
            _reportDAO = reportDAO;
        }

        public async Task<IActionResult> Index()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<YourCoach_ViewModel?> yourCoach = await _yourcoachDAO.DisplayCoach(contextUserId);

            return View("Index", yourCoach);
        }
        public async Task<IActionResult> CoachCheckIns()
        {
            List<WeeklyReport?> weeklyReport = await _reportDAO.GetAllWeeklyReportsOfUser(1);

            return View("CheckIns", weeklyReport);
        }
    }
}
