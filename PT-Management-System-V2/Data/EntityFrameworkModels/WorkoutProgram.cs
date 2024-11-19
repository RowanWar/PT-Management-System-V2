namespace PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;

public partial class WorkoutProgram
{
    public int WorkoutProgramId { get; set; }
    public string ProgramName { get; set; }

    // Length of weeks the program should be run for
    public int ProgramLength { get; set; } 

    // How many sessions per week
    public int WeeklyFrequency { get; set; }

    public string DifficultyLevel { get; set; }

    public string Description { get; set; }

    public string ProgramType { get; set; }

    // Whether the program is a default program or added by a user
    public bool IsDefault { get; set; }

    // The UserId the program was created by
    public string? CreatedByUserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    
    public ICollection<Client> Clients { get; set; }

    public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }

    public ICollection<WorkoutProgramSchedule> WorkoutSchedules { get; set; }
}
