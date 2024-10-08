using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Coach
{
    public int CoachId { get; set; }

    // Foreign key to AspNetUser (Id)
    public string UserId { get; set; }

    public string? CoachProfileDescription { get; set; }

    public string? CoachQualifications { get; set; }

    public virtual Client? CoachClient { get; set; }

    //public ApplicationUser ApplicationUser { get; set; }
}
