using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data.ViewModels
{
    public class WorkoutExercise_Workout_Set_Exercise_ViewModel
    {

        // Exercise Model
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; }

        public string MuscleGroup { get; set; }

        public string Description { get; set; }

        public string IsDefault { get; set; }


        // Set Model
        public int SetId { get; set; }

        public int WorkoutExerciseId { get; set; }

        public int SetCategoryId { get; set; }

        public int Reps { get; set; }

        public Decimal? Weight { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        // Set Category Model
        public int SetCategory { get; set; }

        public string SetCategoryType { get; set; }



        // Workout Model
        public int WorkoutId { get; set; }

        public DateOnly WorkoutDate { get; set; }

        public TimeSpan Duration { get; set; }

        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Boolean? WorkoutActive{ get; set; }

        public string UserId { get; set; }

    }
}
