using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public string ExerciseName { get; set; } = null!;

    public string MuscleGroup { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDefault { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }
}
