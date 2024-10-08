using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PT_Management_System_V2.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PT_Management_System_V2.Data; // Might be redundant
namespace PT_Management_System_V2.Controllers;

public class ClientController : Controller
{
    // Uses DI to bring in DB context
    private readonly ApplicationDbContext _context;
    
    private readonly WorkoutDAO _workoutDAO;
    private readonly ClientDAO _clientDAO;

    // Uses Dependency Injection to implement both WorkoutDAO and ClientDAO. Currently a lot of functionality for this controller is implemented through WorkoutDAO.
    public ClientController(ApplicationDbContext context, WorkoutDAO workoutDAO, ClientDAO clientDAO)
    {
        _context = context;
        _workoutDAO = workoutDAO;
        _clientDAO = clientDAO;
    }



    // Create a list out of the client model so the forEach in the index.cshtml can iterate through all the clients properly.
    //static List<ClientModel> clients = new List<ClientModel>();


    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Index()
    {
        return View(_clientDAO.GetAllClients());
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult TestEndpoint()
    {
        
        return Json(_clientDAO.GetCoachesClients());
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
