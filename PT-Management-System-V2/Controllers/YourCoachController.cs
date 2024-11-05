using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Services;
using System.Security.Claims;
using System.Text.Json;

namespace PT_Management_System_V2.Controllers
{
    public class YourCoachController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;
        private readonly YourCoachDAO _yourCoachDAO;

        public YourCoachController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO, YourCoachDAO yourCoachDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
            _clientDAO = clientDAO;
            _reportDAO = reportDAO;
            _yourCoachDAO = yourCoachDAO;
        }

        public async Task<IActionResult> Index()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);





            // If successful, do next thing

            // Otherwise no valid coach exists and return an oops this doesnt exist status...?


            YourCoach_ViewModel? yourCoach = await _yourCoachDAO.DisplayCoach(contextUserId);

            return View("Index", yourCoach);
        }
        public async Task<IActionResult> CoachCheckIns(int page = 1, int pageSize = 4)
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            // Returns the ClientId of the user from the coach_client table in the db, retrieved from the UserId of the logged in user
            var clientId = await _yourCoachDAO.GetClientIdFromUserId(contextUserId);

            if (clientId == null)
            {
                HttpContent content = null;
            }

            // Calls the Check In code from the reportDAO with the users authenticated clientId from above. This is called in the Index View via a FETCH REQUEST.
            List<WeeklyReport?> weeklyReports = await _reportDAO.GetAllWeeklyReportsOfUser(clientId, page, pageSize);

            //return View("CheckIns", weeklyReport);
            //string jsonSerialized = JsonSerializer.Serialize(weeklyReports);
            
            return Json(weeklyReports);
        }
    }
}
