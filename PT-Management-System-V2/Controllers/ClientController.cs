using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PT_Management_System_V2.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PT_Management_System_V2.Data;
using System.Security.Claims; 


namespace PT_Management_System_V2.Controllers;

public class ClientController : Controller
{
    // Uses DI to bring in DB context
    private readonly ApplicationDbContext _context;
    
    private readonly WorkoutDAO _workoutDAO;
    private readonly ClientDAO _clientDAO;

    // Uses Dependency Injection to implement both WorkoutDAO and ClientDAO. Currently a lot of functionality for this controller is implemented through WorkoutDAO.
    public ClientController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO)
    {
        _context = context;
        _workoutDAO = workoutDAO;
        _clientDAO = clientDAO;
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

        //var clientNames = _context.AspNetUsers
        //    .Select(u => u.Id)
        //    .ToList();
        var clientNames = _context.Clients
            .Select(
                u => new { u.ClientId }
            )
            .ToList();


        return Json(clientNames);
    }

    // Displays a list of all workouts performed by a specific user based upon their UserId in the DB.
    public IActionResult ClientWorkouts(int ClientUserId)
    {
        //List<WorkoutModel> workoutList = _workoutDAO.GetAllWorkoutsByUserId(ClientUserId);
        List<WorkoutExerciseModel> workoutList = _workoutDAO.GetAllWorkoutsByUserId(ClientUserId);

        return View("ClientWorkout", workoutList);
    }

    // Displays the workout details of a user based upon the WorkoutId provided
    public IActionResult WorkoutDetails(int WorkoutId)
    {
        List<WorkoutExercisesModel> workoutDetails = _workoutDAO.GetWorkoutDetailsByWorkoutId(WorkoutId);

        //ViewBag.ModelData = workoutDetails;

        return View("~/Views/Workout/WorkoutDetails.cshtml", workoutDetails);
    }


    public IActionResult WeeklyReport(int ClientUserId)
    {
        List<ClientWeeklyReportModel> weeklyReport = _workoutDAO.GetAllWeeklyReportsByUserId(ClientUserId);

        return View("ClientReport", weeklyReport);
    }

    
    public IActionResult ViewImage(int ReportId)
    {
        List<ImageModel> weeklyReportImages = _workoutDAO.GetAllImagesByWeeklyReportId(ReportId);

        string result = JsonSerializer.Serialize(weeklyReportImages);

        //System.Diagnostics.Debug.WriteLine(result);
        return Json(result.ToString());
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Details(ClientModel client)
    {

        //clients.Add(client);
        //_clientDAO.Add(client);
        return View("Details", client);
    }

    public IActionResult Overview()
    {
        return View();
    }
}
