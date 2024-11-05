using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data.ViewModels
{
    public class ClientCoach_Client_ViewModel
    {
        // WorkoutExercise Model
        public int ClientId { get; set; }

        // Foreign key to AspNetUser (Id)
        public string UserId { get; set; }


        public bool? ContactByPhone { get; set; }

        public bool? ContactByEmail { get; set; }

        public bool? Referred { get; set; }

        public string? Referral { get; set; }



        // Workout Model

        public int MonthlyCharge { get; set; }

        public DateTime ClientStartDate { get; set; }

        public DateTime? ClientEndDate { get; set; }


        // Client Model
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
