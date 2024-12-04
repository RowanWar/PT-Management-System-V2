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
    public class FindCoachController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly FindCoachDAO _findCoachDAO;
        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;

        public FindCoachController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, FindCoachDAO findCoachDAO, WorkoutDAO workoutDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
            _findCoachDAO = findCoachDAO;
        }


        //// Create a list out of the workout model so the forEach in the .cshtml can iterate through all the workouts properly.
        //static List<WorkoutExerciseModel> workouts = new List<WorkoutExerciseModel>();


        // Commented out for migration
        public async Task<IActionResult> Index()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (contextUserId == null)
            {
                return NotFound();
            }

            List<FindCoach_ViewModel> coachList = await _findCoachDAO.GetAllCoaches(contextUserId);

            return View("Index", coachList);
            //return Json(coachList);
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


    }
}
