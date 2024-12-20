﻿using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
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
    public class WorkoutController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly WorkoutDAO _workoutDAO;
        private readonly ClientDAO _clientDAO;
        private readonly ReportDAO _reportDAO;

        public WorkoutController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, WorkoutDAO workoutDAO, ClientDAO clientDAO, ReportDAO reportDAO)
        {
            _context = context;
            _workoutDAO = workoutDAO;
            _clientDAO = clientDAO;
            _reportDAO = reportDAO;
        }


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

        public IActionResult CreateWorkout()
        {

            return View();
        }


        public async Task<IActionResult> ViewActiveWorkoutByUserId()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (contextUserId == null)
            {
                return NotFound();
            }

            List<WorkoutExercise_Workout_Set_Exercise_ViewModel> viewActiveWorkout = await _workoutDAO.ViewActiveWorkoutByUserId(contextUserId);
            string HasActiveWorkoutSerialized = JsonSerializer.Serialize(viewActiveWorkout);


            return Json(HasActiveWorkoutSerialized);
        }


        //public async Task<IActionResult> CreateWorkout(int UserId)
        //{



        //    _workoutDAO exercise = new _workoutDAO();
        //    int result = await exercise.CreateWorkoutInDatabase(WorkoutId, ExerciseIds);

        //    //string resultSerialized = JsonSerializer.Serialize(activeWorkout);

        //    return Json(result);
        //}



        //Grabs the list of ExerciseIds from the POST body
        public async Task<IActionResult> InsertExercises([FromBody] JsonElement data)
        {

            int workoutId = data.GetProperty("WorkoutId").GetInt32();
            List<int> exerciseIds = JsonSerializer.Deserialize<List<int>>(data.GetProperty("ExerciseIds").ToString());
            List<int> setIdsArr = await _workoutDAO.AddExercisesToDatabase(workoutId, exerciseIds);

            // Adds a default empty set to every exercise created by the user for display purposes.
            int result = await _workoutDAO.AddSetToDatabase(setIdsArr);
          

            return Json(setIdsArr);
        }

        // Called when the user clicks the "add set" button in the active workout.
        public async Task<IActionResult> InsertSets(int WorkoutExerciseId)
        {

            // Converts the single ID passed to the function into an array of type integer so we can re-use the function already built in _workoutDAO. 
            // Creates a new int list and adds the parameter ID to it, then passes it to the DAO function.
            List<int> WorkoutExerciseIdArr = new List<int> { WorkoutExerciseId };

            int result = await _workoutDAO.AddSetToDatabase(WorkoutExerciseIdArr);


            return Json(result);
        }

        public async Task<IActionResult> RemoveSets(int SetIds)
        {

            // Converts the single ID passed to the function into an array of type integer so we can re-use the function already built in _workoutDAO. 
            // Creates a new int list and adds the parameter ID to it, then passes it to the DAO function.
            List<int> SetIdsArr = new List<int> { SetIds };
            //_workoutDAO sets = new _workoutDAO();

            int result = await _workoutDAO.RemoveSetsFromDatabase(SetIdsArr);

            return Json(result);
        }


        public IActionResult SubmitExercises(int UserId)
        {
            List<WorkoutExercisesModel> activeWorkout = _workoutDAO.GetUsersActiveWorkout(UserId);

            string resultSerialized = JsonSerializer.Serialize(activeWorkout);

            return Json(resultSerialized);
        }




        // API endpoint for when the user clicks to submit/finish a workout
        public async Task<IActionResult> SubmitWorkout(int UserId, [FromBody] JsonElement WorkoutData)
        {

            int result = await _workoutDAO.FinishUsersActiveWorkout(WorkoutData);

            // If the submitted object has no sets completed, return a message to the user stating such and exit early.
            if (result == 0)
            {
                return Json("ERROR: No sets were submitted for this workout!");
            }

            // Return a successful result.
            return Json(result);
        }




        //Responds to a fetch request, providing a list of exercises from within the "exercise" table in the db
        //Passes in the UserId within the query to also display any custom exercises created by that user in the db
        public async Task<IActionResult> ViewExerciseList()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            List<Exercise_ViewModel> exerciseList = await _workoutDAO.GetAllExercisesByUserId(contextUserId);

            string resultSerialized = JsonSerializer.Serialize(exerciseList);


            return Json(resultSerialized);
        }


        public async Task<IActionResult> CheckForActiveWorkout()
        {
            // Grab the logged in users ID from the user authorization session context
            var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (contextUserId == null)
            {
                return NotFound();
            }


            // Grabs the CoachId from the returned coach object
            var activeWorkoutId = await _workoutDAO.CheckActiveWorkoutByUserId(contextUserId);
            string activeWorkoutIdSerialized = JsonSerializer.Serialize(activeWorkoutId);


            return Json(activeWorkoutIdSerialized);
        }

        //public IActionResult RemoveExerciseFromActiveWorkout(int WorkoutExerciseId)
        //{
        //    // code
        //}

    }
}
