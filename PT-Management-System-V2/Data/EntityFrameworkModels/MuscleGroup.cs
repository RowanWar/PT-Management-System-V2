namespace PT_Management_System_V2.Data.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public partial class MuscleGroup
{
    [Key]
    public int MuscleGroupId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MuscleGroupName { get; set; }


    public ICollection<Exercise> Exercises { get; set; }
}
