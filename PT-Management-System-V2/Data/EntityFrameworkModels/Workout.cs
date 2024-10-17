//using PT_Management_System_V2.Data.Models;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Workout
{
    public int WorkoutId { get; set; }

    public string? WorkoutUserId { get; set; }

    public DateOnly WorkoutDate { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? WorkoutActive { get; set; }

    //[ForeignKey("UserIdTest")]
    public virtual AspNetUser AspNetUser { get; set; }/* = new List<AspNetUser>();*/

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
