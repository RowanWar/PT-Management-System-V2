using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Services;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        //return Json(workoutPrograms);
        return View("Index", workoutPrograms);
    }


    // Responds to fetch request with a json array to populate a dropdown button on page with list of workoutprograms
    public async Task<IActionResult> ListWorkoutPrograms()
    {
        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);





        // If successful, do next thing

        // Otherwise no valid coach exists and return an oops this doesnt exist status...?


        List<WorkoutProgram_ViewModel?> workoutPrograms = await _workoutProgramDAO.DisplayPrograms(contextUserId);

        return Json(workoutPrograms);
    }



    // Updates the workout program assigned to a coach's client 
    [HttpPost]
    public IActionResult UpdateClientsWorkoutProgram(int workoutProgramId, int clientId)
    {

        // If non-valid ID
        if (workoutProgramId <= 0)
        {
            return BadRequest(new { success = false, message = "Invalid or missing program ID!" });
        }


        // Finds the row containing the day of the week via the PK in WorkoutProgramSchedule table 
        var program = _context.WorkoutPrograms.Find(workoutProgramId);
        var client = _context.Clients.Find(clientId);
        if (program != null)
        {
            //program.WorkoutProgramId = workoutProgramId;
            client.WorkoutProgramId = workoutProgramId;
            _context.SaveChanges();
            return Ok(new { success = true, message = "Workout program updated successfully!" });
            //return RedirectToAction("Index");
        }

        // Step 1
        // Set client.workoutprogramid = new programid

        // Step 2
        // Add x number of entries to "GetWorkoutProgramSchedules" dayOfWeek 

        return NotFound();
    }

    //[HttpPost]
    //public IActionResult UpdateClientsWorkoutProgram([FromBody] JsonNode request)
    //{
    //    int workoutProgramId = request["workoutProgramId"]?.GetValue<int>() ?? 0;
    //    //int dayOfWeek = request["dayOfWeek"]?.GetValue<int>() ?? 0;
    //    //string muscleGroup = request["muscleGroup"]?.GetValue<string>();

    //    // If non-valid ID
    //    if (workoutProgramId == 0)
    //    {
    //        return BadRequest(new { success = false, message = "Invalid or missing program ID!" });
    //    }


    //    // Finds the row containing the day of the week via the PK in WorkoutProgramSchedule table 
    //    var program = _context.WorkoutPrograms.Find(workoutProgramId);
    //    if (program != null)
    //    {
    //        program.WorkoutProgramId = workoutProgramId;
    //        _context.SaveChanges();
    //        return Ok();
    //    }
    //    return NotFound();
    //}

}
