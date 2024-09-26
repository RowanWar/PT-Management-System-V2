using PT_Management_System_V2.Services;
using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{
    public class WorkoutModel
    {
        [Key]
        public int WorkoutId { get; set; }

        public int UserId { get; set; }

        public DateTime WorkoutDate { get; set; }

        public TimeSpan Duration { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public Boolean WorkoutActive { get; set; }

    }
}
