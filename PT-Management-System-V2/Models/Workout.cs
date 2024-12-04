using PT_Management_System_V2.Services;
using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{
    public class WorkoutExerciseModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public string UserId { get; set; }

        public string WorkoutName { get; set; }

        public List<WorkoutExercise>? WorkoutExercises { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan Duration { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public Boolean WorkoutActive { get; set; }

    }


}
