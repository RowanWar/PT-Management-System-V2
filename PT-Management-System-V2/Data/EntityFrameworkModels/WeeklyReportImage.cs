using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class WeeklyReportImage
{
    public int WeeklyReportImageId { get; set; }

    public int? WeeklyReportId { get; set; }

    public int? ImageId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual Image? Image { get; set; }

    public virtual WeeklyReport? WeeklyReport { get; set; }
}
