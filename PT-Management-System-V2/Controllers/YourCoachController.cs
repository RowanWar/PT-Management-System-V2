using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Services;

namespace PT_Management_System_V2.Controllers
{
    public class YourCoachController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;

        public YourCoachController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
            _clientDAO = clientDAO;
            _reportDAO = reportDAO;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
