//using System;
//using System.Collections.Generic;

//namespace PT_Management_System_V2.Data.EntityFrameworkModels;

//public partial class User
//{
//    public int UserId { get; set; }

//    public string Username { get; set; } = null!;

//    public string Email { get; set; } = null!;

//    public string Password { get; set; } = null!;

//    public DateTime? CreatedAt { get; set; }

//    public string Firstname { get; set; } = null!;

//    public string Lastname { get; set; } = null!;

//    public string? MobileNumber { get; set; }

//    public DateTime Dob { get; set; }

//    public bool? Active { get; set; }

//    public virtual ICollection<WeeklyReport> WeeklyReports { get; set; } = new List<WeeklyReport>();

//    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
//}
