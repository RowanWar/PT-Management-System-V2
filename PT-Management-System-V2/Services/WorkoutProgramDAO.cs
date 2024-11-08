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
                Exercises = (
                    from wpe in _context.WorkoutProgramExercises
                    join e in _context.Exercises on wpe.ExerciseId equals e.ExerciseId
                    where wpe.WorkoutProgramId == wp.WorkoutProgramId
                    select new Exercise_ViewModel
                    {
                        ExerciseId = e.ExerciseId,
                        ExerciseName = e.ExerciseName,
                        MuscleGroup = e.MuscleGroup,
                        ExerciseDescription = e.Description
                    }).ToList()
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return data;
    }

}
