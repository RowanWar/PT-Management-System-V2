using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{

    public class CoachModel
    {
        [Key]
        public int CoachId { get; set; }

        //[Required]
        public int CoachClientId { get; set; }

        public string CoachProfileDescription { get; set; }

        public string CoachQualifications { get; set; }

        
        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DoB {  get; set; }
    }
}