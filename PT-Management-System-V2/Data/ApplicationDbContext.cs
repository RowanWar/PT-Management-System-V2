using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data.EntityFrameworkModels;

namespace PT_Management_System_V2.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }


    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientMeasurement> ClientMeasurements { get; set; }

    public virtual DbSet<ClientWeight> ClientWeights { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachClient> CoachClients { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<HealthCondition> HealthConditions { get; set; }

    public virtual DbSet<HealthConditionClient> HealthConditionClients { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<SetCategory> SetCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeeklyReport> WeeklyReports { get; set; }

    public virtual DbSet<WeeklyReportImage> WeeklyReportImages { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=BeBetter30;Database=ptsystem");

    // Modelbuilder is for the purpose of Entity Framework Core to know the relationship and how to map entities to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This calls the Microsoft.Identity base models and is essential to ensure authentication works correctly!
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ContactByEmail).HasColumnName("contact_by_email");
            entity.Property(e => e.ContactByPhone).HasColumnName("contact_by_phone");
            entity.Property(e => e.Referral)
                .HasMaxLength(150)
                .HasColumnName("referral");
            entity.Property(e => e.Referred).HasColumnName("referred");


            // Define the FK UserId from Client to AspNetUser (Id)
            entity.HasOne<ApplicationUser>()
                .WithMany() 
                .HasForeignKey(c => c.UserId)
                .IsRequired();
        });



        modelBuilder.Entity<ClientMeasurement>(entity =>
        {
            entity.HasKey(e => e.ClientMeasurementId).HasName("client_measurement_pkey");

            entity.ToTable("client_measurement");

            entity.Property(e => e.ClientMeasurementId).HasColumnName("client_measurement_id");
            entity.Property(e => e.Bicep)
                .HasPrecision(4, 1)
                .HasColumnName("bicep");
            entity.Property(e => e.Chest)
                .HasPrecision(4, 1)
                .HasColumnName("chest");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Hip)
                .HasPrecision(4, 1)
                .HasColumnName("hip");
            entity.Property(e => e.Tricep)
                .HasPrecision(4, 1)
                .HasColumnName("tricep");
            entity.Property(e => e.Waist)
                .HasPrecision(4, 1)
                .HasColumnName("waist");
        });

        modelBuilder.Entity<ClientWeight>(entity =>
        {
            entity.HasKey(e => e.ClientWeightId).HasName("client_weight_pkey");

            entity.ToTable("client_weight");

            entity.Property(e => e.ClientWeightId).HasColumnName("client_weight_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Weight)
                .HasPrecision(4, 1)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.CoachId).HasName("coach_pkey");

            entity.ToTable("coach");

            entity.Property(e => e.CoachId).HasColumnName("coach_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CoachProfileDescription)
                .HasMaxLength(1000)
                .HasColumnName("coach_profile_description");
            entity.Property(e => e.CoachQualifications)
                .HasMaxLength(500)
                .HasColumnName("coach_qualifications");

            //entity.HasOne(coach => coach.CoachClient).WithMany(client => client.Coaches)
            //    .HasForeignKey(coach => coach.UserId)
            //    .HasConstraintName("coach_coach_user_id_fkey");


            // Define the FK UserId from Coach (UserId) to AspNetUsers (Id)
            entity.HasOne<ApplicationUser>()
                .WithMany()                
                .HasForeignKey(coach => coach.UserId)
                .HasConstraintName("Coach_AspNetUsers_Id_Fkey")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<CoachClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("coach_client");

            entity.Property(e => e.ClientEndDate).HasColumnName("client_end_date");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ClientStartDate).HasColumnName("client_start_date");
            entity.Property(e => e.CoachId).HasColumnName("coach_id");
            entity.Property(e => e.MonthlyCharge).HasColumnName("monthly_charge");

            entity.HasOne(d => d.Client).WithMany()
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("coach_client_client_id_fkey");

            entity.HasOne(d => d.Coach).WithMany()
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("coach_client_coach_id_fkey");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("exercise_pkey");

            entity.ToTable("exercise");

            entity.HasIndex(e => e.ExerciseName, "exercise_exercise_name_key").IsUnique();

            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(100)
                .HasColumnName("exercise_name");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.MuscleGroup)
                .HasMaxLength(100)
                .HasColumnName("muscle_group");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<HealthCondition>(entity =>
        {
            entity.HasKey(e => e.HealthConditionId).HasName("health_condition_pkey");

            entity.ToTable("health_condition");

            entity.Property(e => e.HealthConditionId).HasColumnName("health_condition_id");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.HealthCondition1)
                .HasMaxLength(250)
                .HasColumnName("health_condition");
        });

        modelBuilder.Entity<HealthConditionClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("health_condition_client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.HealthConditionId).HasColumnName("health_condition_id");

            entity.HasOne(d => d.Client).WithMany()
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("health_condition_client_client_id_fkey");

            entity.HasOne(d => d.HealthCondition).WithMany()
                .HasForeignKey(d => d.HealthConditionId)
                .HasConstraintName("health_condition_client_health_condition_id_fkey");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.FilePath)
                .HasMaxLength(300)
                .HasColumnName("file_path");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.SetId).HasName("set_pkey");

            entity.ToTable("set");

            entity.Property(e => e.SetId).HasColumnName("set_id");
            entity.Property(e => e.Endtime).HasColumnName("endtime");
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.SetCategoryId).HasColumnName("set_category_id");
            entity.Property(e => e.Starttime).HasColumnName("starttime");
            entity.Property(e => e.Weight)
                .HasPrecision(8, 2)
                .HasColumnName("weight");
            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workout_exercise_id");

            entity.HasOne(d => d.SetCategory).WithMany(p => p.Sets)
                .HasForeignKey(d => d.SetCategoryId)
                .HasConstraintName("set_set_category_id_fkey");

            entity.HasOne(d => d.WorkoutExercise).WithMany(p => p.Sets)
                .HasForeignKey(d => d.WorkoutExerciseId)
                .HasConstraintName("set_workout_exercise_id_fkey");
        });

        modelBuilder.Entity<SetCategory>(entity =>
        {
            entity.HasKey(e => e.SetCategoryId).HasName("set_category_pkey");

            entity.ToTable("set_category");

            entity.Property(e => e.SetCategoryId).HasColumnName("set_category_id");
            entity.Property(e => e.SetCategoryType)
                .HasMaxLength(25)
                .HasColumnName("set_category_type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .HasColumnName("mobile_number");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<WeeklyReport>(entity =>
        {
            entity.HasKey(e => e.WeeklyReportId).HasName("weekly_report_pkey");

            entity.ToTable("weekly_report");

            entity.Property(e => e.WeeklyReportId).HasColumnName("weekly_report_id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckInWeight)
                .HasPrecision(4, 1)
                .HasColumnName("check_in_weight");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.WeeklyReports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("weekly_report_user_id_fkey");
        });

        modelBuilder.Entity<WeeklyReportImage>(entity =>
        {
            entity.HasKey(e => e.WeeklyReportImageId).HasName("weekly_report_image_pkey");

            entity.ToTable("weekly_report_image");

            entity.Property(e => e.WeeklyReportImageId).HasColumnName("weekly_report_image_id");
            entity.Property(e => e.DateCreated).HasColumnName("date_created");
            entity.Property(e => e.DateDeleted).HasColumnName("date_deleted");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.WeeklyReportId).HasColumnName("weekly_report_id");

            entity.HasOne(d => d.Image).WithMany(p => p.WeeklyReportImages)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("weekly_report_image_image_id_fkey");

            entity.HasOne(d => d.WeeklyReport).WithMany(p => p.WeeklyReportImages)
                .HasForeignKey(d => d.WeeklyReportId)
                .HasConstraintName("weekly_report_image_weekly_report_id_fkey");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("workout_pkey");

            entity.ToTable("workout");

            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WorkoutActive).HasColumnName("workout_active");
            entity.Property(e => e.WorkoutDate).HasColumnName("workout_date");

            entity.HasOne(d => d.User).WithMany(p => p.Workouts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("workout_user_id_fkey");
        });

        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutExerciseId).HasName("workout_exercise_pkey");

            entity.ToTable("workout_exercise");

            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workout_exercise_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("workout_exercise_exercise_id_fkey");

            entity.HasOne(d => d.Workout).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.WorkoutId)
                .HasConstraintName("workout_exercise_workout_id_fkey");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
