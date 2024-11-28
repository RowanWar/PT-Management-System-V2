using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Models;
using System.Drawing.Printing;

namespace PT_Management_System_V2.Services;

public class WorkoutProgramDAO
{

    private readonly string _dbConnectionString;
    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; 


    public WorkoutProgramDAO(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }



    // Returns a list of workout programs, including programs the coach has created themselves.
    public async Task<List<WorkoutProgram_ViewModel?>> DisplayPrograms(string contextUserId, int page = 1, int pageSize = 10)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var data = await (
            from wp in _context.WorkoutPrograms
            where wp.CreatedByUserId == contextUserId || wp.IsDefault == true
            select new WorkoutProgram_ViewModel
            {
                WorkoutProgramId = wp.WorkoutProgramId,
                ProgramName = wp.ProgramName,
                ProgramLength = wp.ProgramLength,
                WeeklyFrequency = wp.WeeklyFrequency,
                DifficultyLevel = wp.DifficultyLevel,
                ProgramDescription = wp.Description,
                ProgramType = wp.ProgramType,
                IsDefault = wp.IsDefault,
                StartDate = wp.StartDate,
                EndDate = wp.EndDate,

                // Creates a separate list for exercises within a program which are iterated through on the Razor view. This reduces data redundancy and should help improve page load times.
                //Exercises = (
                //    from wpe in _context.WorkoutProgramExercises
                //    join e in _context.Exercises on wpe.ExerciseId equals e.ExerciseId
                //    where wpe.WorkoutProgramId == wp.WorkoutProgramId
                //    select new Exercise_ViewModel
                //    {
                //        ExerciseId = e.ExerciseId,
                //        ExerciseName = e.ExerciseName,
                //        MuscleGroup = e.MuscleGroup,
                //        ExerciseDescription = e.Description
                //    }).ToList()
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return data;
    }


    // Used to lazy-load data individually based on which program has been focused/selected in the workoutprogram/index page
    public async Task<List<Exercise_ViewModel?>> DisplayExercisesByProgramId(string contextUserId, int workoutProgramId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var data = await (
            from wp in _context.WorkoutPrograms
            where wp.CreatedByUserId == contextUserId || wp.IsDefault == true
            join wpe in _context.WorkoutProgramExercises on workoutProgramId equals wpe.WorkoutProgramId
            join e in _context.Exercises on wpe.ExerciseId equals e.ExerciseId
            where wpe.WorkoutProgramId == wp.WorkoutProgramId
            select new Exercise_ViewModel
            {   
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                MuscleGroup = e.MuscleGroup,
                ExerciseDescription = e.Description
            })
            .ToListAsync();

        return data;
    }
    


    public async Task<bool> DeleteExerciseOnWorkoutProgramId(string contextUserId, int workoutProgramId, int exerciseId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var exerciseToDelete = await _context.WorkoutProgramExercises
            .FirstOrDefaultAsync(wpe => wpe.WorkoutProgramId == workoutProgramId && wpe.ExerciseId == exerciseId);

        if (exerciseToDelete == null)
        {
            return false;
        }

        _context.WorkoutProgramExercises.Remove(exerciseToDelete);
        await _context.SaveChangesAsync();

        // Returns bool as true if succesful in deleting data
        return true;
    }


    // WORKING HERE
    public async Task<bool> AddExerciseOnWorkoutProgramId(string contextUserId, int workoutProgramId, int exerciseId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var exerciseToAdd = await _context.WorkoutProgramExercises
            .FirstOrDefaultAsync(wpe => wpe.WorkoutProgramId == workoutProgramId && wpe.ExerciseId == exerciseId);

        if (exerciseToAdd == null)
        {
            return false;
        }

        _context.WorkoutProgramExercises.Remove(exerciseToDelete);
        await _context.SaveChangesAsync();

        // Returns bool as true if succesful in deleting data
        return true;
    }



    // Assigns a program workout schedule to a user  
    public async Task<int?> AssignProgramWorkoutSchedule(string contextUserId, int clientId, int workoutProgramId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var clientToUpdate = await (
            from wp in _context.WorkoutPrograms
            where wp.WorkoutProgramId == workoutProgramId
            join c in _context.Clients on clientId equals c.ClientId
            select new
            {
                Client = c,
                WeeklyFrequency = wp.WeeklyFrequency
            })
            .FirstOrDefaultAsync();

        if (clientToUpdate == null)
        {
            throw new Exception("ERROR: Client or Workout Program not found!");
        }

        clientToUpdate.Client.WorkoutProgramId = workoutProgramId;
        await _context.SaveChangesAsync();

        // Returns an int of number of days per week that the new workout program is
        return clientToUpdate.WeeklyFrequency;
    }



    public async Task<bool> AddMuscleGroupsToProgramSchedule(string contextUserId, int workoutProgramId, int weeklyFrequency)
    {
        using var _context = _contextFactory.CreateDbContext();

        // Fetch existing schedules for the given WorkoutProgramId
        var existingSchedules = await _context.WorkoutProgramSchedules
            .Where(s => s.WorkoutProgramId == workoutProgramId)
            .ToListAsync();

        // Remove all existing schedules for this WorkoutProgramId
        _context.WorkoutProgramSchedules.RemoveRange(existingSchedules);

        // Create new rows in table based on number of days in the program
        var newSchedules = new List<WorkoutProgramSchedule>();
        for (int i = 0; i < weeklyFrequency; i++)
        {
            newSchedules.Add(new WorkoutProgramSchedule
            {
                WorkoutProgramId = workoutProgramId,
                DayOfWeek = i % 7, // Cycling through days of the week (0 = Sunday, 6 = Saturday)
                MuscleGroupId = 1, 
            });
        }

        // Add new schedules to the database
        await _context.WorkoutProgramSchedules.AddRangeAsync(newSchedules);
        await _context.SaveChangesAsync();

        return true;
    }

}
