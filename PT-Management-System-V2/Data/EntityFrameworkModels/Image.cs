using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Image
{
    public int ImageId { get; set; }

    public string? FilePath { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual ICollection<WeeklyReportImage> WeeklyReportImages { get; set; } = new List<WeeklyReportImage>();
}
