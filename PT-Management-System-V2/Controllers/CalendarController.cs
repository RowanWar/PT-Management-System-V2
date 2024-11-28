using Azure.Core;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Models;
using PT_Management_System_V2.Services;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes; // Might be redundant


namespace PT_Management_System_V2.Controllers

{
    public class CalendarController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;

        public CalendarController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
        }


        //// Create a list out of the workout model so the forEach in the .cshtml can iterate through all the workouts properly.
        //static List<WorkoutExerciseModel> workouts = new List<WorkoutExerciseModel>();


        // Commented out for migration
        public IActionResult Index()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (contextUserId == null)
            {
                return NotFound();
            }
  

            return View(_workoutDAO.GetAllWorkoutsByUserId(contextUserId));
        }


        [HttpGet]
        public async Task<IActionResult> GetDaysOfWeek([FromQuery] int workoutProgramId)
        {

            var daysOfWeek = await(
                from wps in _context.WorkoutProgramSchedules
                where wps.WorkoutProgramId == workoutProgramId
                join mg in _context.MuscleGroups on wps.MuscleGroupId equals mg.MuscleGroupId
                select new
                {
                    id = wps.WorkoutScheduleId, // Might need to be workoutScheduleId
                    daysOfWeek = "[" + wps.DayOfWeek + "]",
                    title = mg.MuscleGroupName
                }).ToListAsync();


            return Json(daysOfWeek);
        }



        // Responsible for updating in db if workout is dragged to a new day
        [HttpPost]
        public IActionResult UpdateWorkoutProgram([FromBody] JsonNode request)
        {
            int workoutScheduleId = request["workoutScheduleId"]?.GetValue<int>() ?? 0;
            int dayOfWeek = request["dayOfWeek"]?.GetValue<int>() ?? 0;
            string muscleGroup = request["muscleGroup"]?.GetValue<string>();

            if (workoutScheduleId == 0 || dayOfWeek == 0 /*|| muscleGroup.IsNullOrEmpty()*/)
            {
                return BadRequest(new { success = false, message = "Invalid or missing workout properties!" });
            }


            // Finds the row containing the day of the week via the PK in WorkoutProgramSchedule table 
            var muscleGroupDay = _context.WorkoutProgramSchedules.Find(workoutScheduleId);
            if (muscleGroupDay != null)
            {
                muscleGroupDay.DayOfWeek = dayOfWeek;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }



        //public async Task AddWorkoutScheduleAsync(int programId, int dayOfWeek, string muscleGroup)
        //{
        //    var schedule = new WorkoutSchedule
        //    {
        //        WorkoutProgramId = programId,
        //        DayOfWeek = dayOfWeek,
        //        MuscleGroup = muscleGroup
        //    };

        //    _context.WorkoutSchedules.Add(schedule);
        //    await _context.SaveChangesAsync();
        //}


        // Copy paste from previous controller for reference - not to be left here
        //public async Task<IActionResult> ViewActiveWorkoutByUserId()
        //{
        //    // Grab the logged in users ID from the user authorization session context
        //    var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (contextUserId == null)
        //    {
        //        return NotFound();
        //    }

        //    List<WorkoutExercise_Workout_Set_Exercise_ViewModel> viewActiveWorkout = await _workoutDAO.ViewActiveWorkoutByUserId(contextUserId);
        //    string HasActiveWorkoutSerialized = JsonSerializer.Serialize(viewActiveWorkout);


        //    return Json(HasActiveWorkoutSerialized);
        //}


    }
}
