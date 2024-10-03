using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Set
{
    public int SetId { get; set; }

    public int? WorkoutExerciseId { get; set; }

    public int? SetCategoryId { get; set; }

    public int Reps { get; set; }

    public decimal? Weight { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public virtual SetCategory? SetCategory { get; set; }

    public virtual WorkoutExercise? WorkoutExercise { get; set; }
}


public class ClientCoach
{
    public Client client { get; set; }

    public Coach coach { get; set; }


}