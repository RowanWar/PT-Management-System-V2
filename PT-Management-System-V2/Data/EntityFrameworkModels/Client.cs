using System;
using System.Collections.Generic;

namespace PT_Management_System_V2.Data.EntityFrameworkModels;

public partial class Client
{
    public int ClientId { get; set; }

    // Foreign key to AspNetUser (Id)
    public string UserId { get; set; }

    // Nullable as not all clients will have programs assigned immediately
    public int? WorkoutProgramId { get; set; } 

    public string ApplicationUserId { get; set; }  // Redundant temp fix...

    public bool? ContactByPhone { get; set; }

    public bool? ContactByEmail { get; set; }

    public bool? Referred { get; set; }

    public string? Referral { get; set; }



    // Navigation property to the intersection table for coaches and clients
    public ICollection<CoachClient> CoachClients { get; set; }

    public ICollection<WeeklyReport> WeeklyReports { get; set; }

    public ApplicationUser User { get; set; }

    // Navigation property to a workout
    public WorkoutProgram WorkoutProgram { get; set; }
}
