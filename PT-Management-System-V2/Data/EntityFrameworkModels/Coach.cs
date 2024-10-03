using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Coach
{
    public int CoachId { get; set; }

    public int? CoachClientId { get; set; }

    public string? CoachProfileDescription { get; set; }

    public string? CoachQualifications { get; set; }

    public virtual Client? CoachClient { get; set; }
}
