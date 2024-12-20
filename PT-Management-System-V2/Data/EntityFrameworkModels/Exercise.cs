﻿using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public string ExerciseName { get; set; } = null!;

    public string MuscleGroup { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsDefault { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    public ICollection<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }

    // FK to AspNetUsers table
    public virtual ApplicationUser? User { get; set; }

    // FK to MuscleGroup table
    public int MuscleGroupId { get; set; }

    public MuscleGroup ExerciseMuscleGroup { get; set; }
}
