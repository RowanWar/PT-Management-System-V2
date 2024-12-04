using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data.RequestModels
{
    public class AddExerciseToProgramRequest_Request
    {

        public int WorkoutProgramId { get; set; }
        public List<int> ExerciseIds { get; set; }

    }
}
