using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class WorkoutExercise
{
    public int WorkoutExerciseId { get; set; }

    public int? WorkoutId { get; set; }

    public int? ExerciseId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Exercise? Exercise { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    public virtual Workout? Workout { get; set; }
}
