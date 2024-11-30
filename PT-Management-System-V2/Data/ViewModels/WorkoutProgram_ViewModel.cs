using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data.ViewModels;

public class WorkoutProgram_ViewModel
{
    // WorkoutProgram Model
    public int WorkoutProgramId { get; set; }
    public string ProgramName { get; set; }

    // Length of weeks the program should be run for
    public int ProgramLength { get; set; }

    // How many sessions per week
    public int WeeklyFrequency { get; set; }

    public string DifficultyLevel { get; set; }

    public string ProgramDescription { get; set; }

    public string ProgramType { get; set; }

    // Whether the program is a default program or added by a user
    public bool IsDefault { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<Exercise_ViewModel> Exercises { get; set; }

}

public class Exercise_ViewModel
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; }
    public string MuscleGroup { get; set; }
    public string ExerciseDescription { get; set; }
    public bool IsDefault { get; set; }

    public string UserId { get; set; }
}