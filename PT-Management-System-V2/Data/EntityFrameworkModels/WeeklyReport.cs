using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class WeeklyReport
{
    public int WeeklyReportId { get; set; }

    public int? ClientId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CheckInDate { get; set; }

    public decimal? CheckInWeight { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual Client? Client{ get; set; }

    public virtual ICollection<WeeklyReportImage> WeeklyReportImages { get; set; } = new List<WeeklyReportImage>();
}
