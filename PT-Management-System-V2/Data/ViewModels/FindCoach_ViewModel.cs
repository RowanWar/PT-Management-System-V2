using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data.ViewModels
{
    //[Keyless]
    public class FindCoach_ViewModel
    {
        // AspNetUser Model
        public string? CoachFirstName { get; set; }

        public string? CoachLastName { get; set; }

        public string? CoachUserName { get; set; }

        public string? CoachEmail { get; set; }

        public string? CoachPhoneNumber { get; set; }


        // Coach Model 
        public string? CoachProfileDescription { get; set; }

        public string? CoachQualifications { get; set; }


    }
}
