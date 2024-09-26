using Microsoft.AspNetCore.Mvc;
using PT_Management_System_V2.Models;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PT_Management_System_V2.Services;
using System.Text.Json; // Might be redundant
namespace PT_Management_System_V2.Controllers
{
    public class ClientController : Controller
    {

        WorkoutDAO WorkoutDAO = new WorkoutDAO(null);
        // Create a list out of the client model so the forEach in the index.cshtml can iterate through all the clients properly.
        static List<ClientModel> clients = new List<ClientModel>();
        public IActionResult Index()
        {
            ClientDAO clients = new ClientDAO();

            return View(clients.GetAllClients());
        }

        // Displays a list of all workouts performed by a specific user based upon their UserId in the DB.
        public IActionResult ClientWorkouts(int ClientUserId)
        {
            List<WorkoutModel> workoutList = WorkoutDAO.GetAllWorkoutsByUserId(ClientUserId);

            return View("ClientWorkout", workoutList);
        }

        // Displays the workout details of a user based upon the WorkoutId provided
        public IActionResult WorkoutDetails(int WorkoutId)
        {
            List<WorkoutExercisesModel> workoutDetails = WorkoutDAO.GetWorkoutDetailsByWorkoutId(WorkoutId);

            //ViewBag.ModelData = workoutDetails;

            return View("~/Views/Workout/WorkoutDetails.cshtml", workoutDetails);
        }


        public IActionResult WeeklyReport(int ClientUserId)
        {
            List<ClientWeeklyReportModel> weeklyReport = WorkoutDAO.GetAllWeeklyReportsByUserId(ClientUserId);

            return View("ClientReport", weeklyReport);
        }

        
        public IActionResult ViewImage(int ReportId)
        {
            List<ImageModel> weeklyReportImages = WorkoutDAO.GetAllImagesByWeeklyReportId(ReportId);

            string result = JsonSerializer.Serialize(weeklyReportImages);

            //System.Diagnostics.Debug.WriteLine(result);
            return Json(result.ToString());
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(ClientModel client)
        {

            clients.Add(client);
            return View("Details", client);
        }

        public IActionResult Overview()
        {
            return View();
        }
    }
}
