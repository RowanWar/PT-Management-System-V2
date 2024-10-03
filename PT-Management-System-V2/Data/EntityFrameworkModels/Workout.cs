using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Workout
{
    public int WorkoutId { get; set; }

    public int? UserId { get; set; }

    public DateOnly WorkoutDate { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? WorkoutActive { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
