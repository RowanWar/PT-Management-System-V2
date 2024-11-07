using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Services;
using System.Security.Claims;
using System.Text.Json;

namespace PT_Management_System_V2.Controllers;

public class WorkoutProgramController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly WorkoutDAO _workoutDAO;
    private readonly ClientDAO _clientDAO;
    private readonly ReportDAO _reportDAO;
    private readonly YourCoachDAO _yourCoachDAO;
    private readonly WorkoutProgramDAO _workoutProgramDAO;

    public WorkoutProgramController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO, YourCoachDAO yourCoachDAO, WorkoutProgramDAO workoutProgramDAO)
    {
        _context = context;
        _workoutDAO = workoutDAO;
        _clientDAO = clientDAO;
        _reportDAO = reportDAO;
        _yourCoachDAO = yourCoachDAO;
        _workoutProgramDAO = workoutProgramDAO;
    }

    public async Task<IActionResult> Index()
    {
        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);





        // If successful, do next thing

        // Otherwise no valid coach exists and return an oops this doesnt exist status...?


        List<WorkoutProgram_ViewModel?> workoutPrograms = await _workoutProgramDAO.DisplayPrograms(contextUserId);

        return Json(workoutPrograms);
        //return View("Index", workoutPrograms);
    }

}
