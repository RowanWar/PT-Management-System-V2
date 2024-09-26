using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{

    public class ClientModel
    {
        [Key]
        [DisplayName("Client ID")] // Not implemented yet.
        public int ClientId { get; set; }

        [DisplayName("User ID")]
        public int ClientUserId { get; set; }


        [DisplayName("First name")]
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string ClientFirstName { get; set; }

        [DisplayName("Last name")]
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string ClientLastName { get; set; }

        [DisplayName("Email Address")]
        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string ClientEmail { get; set; }

        [DisplayName("Username")]
        public string ClientUsername { get; set; }

        [DisplayName("Phone No")]
        public string ClientPhone { get; set; }
        //[DisplayName("Scheduled check-in date")]
        //[DataType(DataType.Date)]
        //public DateTime CheckInDate { get; set; }

        //[DisplayName("Client start date")]
        //[Required]
        //public DateTime InitiationDate { get; set; }

        //[DisplayName("Monthly charge")]
        //[Required]
        //[DataType(DataType.Currency)]
        //[Range(0, 999)]
        //public int MonthlyCost { get; set; }
    }
}

//val = (int)result.GetValue(0);