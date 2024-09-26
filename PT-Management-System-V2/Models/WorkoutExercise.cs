using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PT_Management_System_V2.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PT_Management_System_V2.Models
{
    public class WorkoutExercise
    {
        [Key]
        public int WorkoutExerciseId { get; set; }

        public int WorkoutId { get; set; }

        public int ExerciseId { get; set; }

        public List<WorkoutSet>? WorkoutSet { get; set; }

        [DisplayName("Notes")]
        public string? Notes { get; set; }
    }

    public class WorkoutExerciseViewModel {
        public WorkoutExercise WorkoutExercise { get; set; }

        public ExerciseModel Exercise { get; set; }
    }


    public class WorkoutSet
    {
        [Key]
        public int SetId { get; set; }

        public int WorkoutExerciseId { get; set; }

        [DisplayName("Category")]
        public int SetCategoryId { get; set; }

        [DisplayName("Weight")]
        public decimal Weight { get; set; }

        [DisplayName("Reps")]
        public int Reps { get; set; }

    }
}
