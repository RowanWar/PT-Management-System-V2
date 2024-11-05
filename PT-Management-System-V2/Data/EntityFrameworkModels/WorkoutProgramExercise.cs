namespace PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;

public partial class WorkoutProgramExercise
{
    public int WorkoutProgramId { get; set; }
    public WorkoutProgram WorkoutProgram { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
}
