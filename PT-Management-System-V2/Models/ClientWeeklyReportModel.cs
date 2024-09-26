using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{
    public class ClientWeeklyReportModel
    {
        [Key]
        public int WeeklyReportId {  get; set; }

        public int UserId { get; set; }

        public string ReportNotes { get; set; }

        public DateTime CheckInDate { get; set; }

        public Decimal CheckInWeight { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateDeleted { get; set; } // Nullable DateTime

    }
}
