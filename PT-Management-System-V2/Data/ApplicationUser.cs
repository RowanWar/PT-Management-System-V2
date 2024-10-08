using Microsoft.AspNetCore.Identity;

namespace PT_Management_System_V2.Data;

// Extends the default Microsoft Identity user with custom attributes / columns

public class ApplicationUser : IdentityUser
{
    // These columns are nullable for now. DateCreated needs to be created in the DB on initilization, can add later.
    public DateTime? DateCreated { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? AccountActive { get; set; }
}
