using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PT_Management_System_V2.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PT_Management_System_V2.Data;
using System.Security.Claims;
using PT_Management_System_V2.Data.EntityFrameworkModels;


namespace PT_Management_System_V2.Controllers;

public class ClientController : Controller
{
    // Uses DI to bring in DB context
    private readonly ApplicationDbContext _context;
    
    private readonly WorkoutDAO _workoutDAO;
    private readonly ClientDAO _clientDAO;
    private readonly ReportDAO _reportDAO;

    // Uses Dependency Injection to implement both WorkoutDAO and ClientDAO. Currently a lot of functionality for this controller is implemented through WorkoutDAO.
    public ClientController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO)
    {
        _context = context;
        _workoutDAO = workoutDAO;
        _clientDAO = clientDAO;
        _reportDAO = reportDAO;
    }



    // Create a list out of the client model so the forEach in the index.cshtml can iterate through all the clients properly.
    //static List<ClientModel> clients = new List<ClientModel>(); // P sure this is redundant/obsolete and can be deleted


    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Policy = "CoachPolicy")]
    public async Task<IActionResult> Index()
    {
        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Returns the associated coach_id of a provided user_id, if exists
        var coachId = await _clientDAO.VerifyAndGetUsersCoachId(contextUserId);

        // This error handling needs to be cleaned up to handle if user has no clients vs is not a coach
        if (coachId == null)
        {
            return BadRequest("You are not registered as a coach or lack clients");
        }

        // Grabs the CoachId from the returned coach object
        var retrievedClients = await _clientDAO.GetAllClients(coachId.CoachId);

        return View("Index", retrievedClients);
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "CoachPolicy")]
    [HttpGet()]
    public IActionResult TestEndpoint([FromQuery] string client_id)
    {

        var clientNames = _context.Clients
            .Select(
                u => new { u.ClientId }
            )
            .ToList();


        return Json(clientNames);
    }

    // Displays a list of all workouts performed by a specific user based upon their UserId in the DB.
    [Authorize(Policy = "CoachPolicy")]
    [HttpGet()]
    public IActionResult ClientWorkouts(int ClientId)
    {
        List<WorkoutExerciseModel> workoutList = _workoutDAO.GetAllWorkoutsByUserId(ClientId);

        return View("ClientWorkout", workoutList);
    }

    // Displays the workout details of a user based upon the WorkoutId provided
    public IActionResult WorkoutDetails(int WorkoutId)
    {
        List<WorkoutExercisesModel> workoutDetails = _workoutDAO.GetWorkoutDetailsByWorkoutId(WorkoutId);

        return View("~/Views/Workout/WorkoutDetails.cshtml", workoutDetails);
    }

    [Authorize(Policy = "CoachPolicy")]
    [HttpGet()]
    public async Task<IActionResult> WeeklyReport([FromQuery] int ClientId)
    {
        List<WeeklyReport?> weeklyReport = await _reportDAO.GetAllWeeklyReportsOfUser(ClientId);

        return View("ClientReport", weeklyReport);
    }

    
    public IActionResult ViewImage(int ReportId)
    {
        List<ImageModel> weeklyReportImages = _workoutDAO.GetAllImagesByWeeklyReportId(ReportId);

        string result = JsonSerializer.Serialize(weeklyReportImages);

        return Json(result.ToString());
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Details(ClientModel client)
    {

        return View("Details", client);
    }

    public IActionResult Overview()
    {
        return View();
    }
}
