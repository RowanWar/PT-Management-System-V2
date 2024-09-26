using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services
{
    public interface IWorkoutDataService
    {
        List<WorkoutModel> GetAllWorkouts();
        List<WorkoutModel> SearchWorkouts(string searchTerm);

        WorkoutModel GetWorkoutById(int id);

        // Lists all of a specific users workouts for a coach to view their clients training.
        List<WorkoutModel> GetAllWorkoutsByUserId(int UserId);

        List<ClientWeeklyReportModel> GetAllWeeklyReportsByUserId(int UserId);


        List<ImageModel> GetAllImagesByWeeklyReportId(int UserId);
        //int Insert(WorkoutModel workout);
        //int Update(WorkoutModel workout);

        //int Delete(WorkoutModel workout);
    }
}
