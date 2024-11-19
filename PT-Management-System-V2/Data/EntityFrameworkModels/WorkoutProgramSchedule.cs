﻿namespace PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public partial class WorkoutProgramSchedule
{
    [Key]
    public int WorkoutScheduleId { get; set; }

    [Required]
    public int WorkoutProgramId { get; set; }

    [Required]
    [Range(1, 7, ErrorMessage = "Day of week must be between 1 (Monday) and 7 (Sunday)")]

    public int DayOfWeek { get; set; }  

    [Required]
    [MaxLength(50)]
    public string MuscleGroup { get; set; } = string.Empty;

    [ForeignKey("WorkoutProgramId")]
    public WorkoutProgram WorkoutProgram { get; set; } 
}
