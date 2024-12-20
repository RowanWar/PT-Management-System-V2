﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.RequestModels;
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

        List<WorkoutProgram_ViewModel?> workoutPrograms = await _workoutProgramDAO.DisplayPrograms(contextUserId);

        return Json(workoutPrograms);
    }


    // Utilised for AJAX fetch request to lazy-load exercises within a program once clicked on
    public async Task<IActionResult> ListWorkoutExercises([FromQuery] int workoutProgramId)
    {
        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<Exercise_ViewModel?> programExercises = await _workoutProgramDAO.DisplayExercisesByProgramId(contextUserId, workoutProgramId);

        return Json(programExercises);
    }


    [HttpPost]
    public async Task<IActionResult> DeleteExerciseFromWorkoutProgram([FromBody] JsonNode request)
    {

        // Might need to change these to include data- at the front
        int workoutProgramId = request["workoutProgramId"]?.GetValue<int>() ?? 0;
        int exerciseId = request["exerciseId"]?.GetValue<int>() ?? 0;


        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If non-valid ID
        if (workoutProgramId <= 0)
        {
            return BadRequest(new { success = false, message = "Invalid or missing program ID!" });
        }


        // Returns a boolean dictating whether the exercise was successfully deleted from the workout program id provided
        bool isDeleted = await _workoutProgramDAO.DeleteExerciseOnWorkoutProgramId(contextUserId, workoutProgramId, exerciseId);

        if (isDeleted)
        {

            return Ok(new { success = true, message = "Workout program updated successfully!" });
        }


        return Ok(new { success = false });
    }


    // Adds one or more exercises to a workout program based on a list of exerciseId(s) passed to it
    [HttpPost]
    public async Task<IActionResult> AddExerciseToWorkoutProgram([FromBody] AddExerciseToProgramRequest_Request request)
    {

        if (request.WorkoutProgramId <= 0)
        {
            return BadRequest(new { success = false, message = "Invalid or missing program ID!" });
        }

        if (request.ExerciseIds == null || !request.ExerciseIds.Any())
        {
            return BadRequest(new { success = false, message = "No valid exercises provided!" });
        }



        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            // Returns a list of details pertaining to the exercises added to this program for dynamic updating/display on the front-end
            List<Exercise_ViewModel> addedExerciseDetails = await _workoutProgramDAO.AddExerciseOnWorkoutProgramId(contextUserId, request.WorkoutProgramId, request.ExerciseIds);

            return Ok(new
            {
                success = true,
                message = "Exercises added successfully!",
                exercises = addedExerciseDetails
            });
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }


    // Updates the workout program assigned to a coach's client 
    [HttpPost]
    public async Task<IActionResult> UpdateClientsWorkoutProgram([FromBody] JsonNode request)
    {

        int workoutProgramId = request["workoutProgramId"]?.GetValue<int>() ?? 0;
        int clientId = request["clientId"]?.GetValue<int>() ?? 0;


        // Grab the logged in users ID from the user authorization session context
        var contextUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If non-valid ID
        if (workoutProgramId <= 0)
        {
            return BadRequest(new { success = false, message = "Invalid or missing program ID!" });
        }


        // If succesful in updating clients workout program, returns the number of days the new program is which is used in the function below to add x num of days to the ProgramSchedule table
        int? weeklyFrequency = await _workoutProgramDAO.AssignProgramWorkoutSchedule(contextUserId, clientId, workoutProgramId);

        return Ok(new { success = true, message = "Workout program updated successfully!" });



        // I don't think this is required in this controller. Move this to a seperate thing when modifying the program itself? 
        //if (weeklyFrequency.HasValue)
        //{
        //    bool isUpdated = await _workoutProgramDAO.AddMuscleGroupsToProgramSchedule(contextUserId, workoutProgramId, (int)weeklyFrequency);

        //    if (isUpdated)
        //    {
        //        //return Ok(new { success = true, message = "Workout program and schedule updated successfully!" });
        //        return Ok();
        //    }
        //}

    }


}
