using Npgsql;
//using NuGet.Protocol.Plugins;
using PT_Management_System_V2.Models;
using static Azure.Core.HttpHeader;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using System.Numerics;
using System.Text.Json;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Internal;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.ViewModels;

namespace PT_Management_System_V2.Services;


public class WorkoutDAO /*: IWorkoutDataService*/ 
{
    private readonly string _dbConnectionString;
    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    public WorkoutDAO(string dbConnectionString, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _dbConnectionString = dbConnectionString;
        _contextFactory = contextFactory;
    }

    // [FOR COACH] 
    public async Task<List<Workout?>> GetAllWorkoutsByClientId(int clientId)
    {
        using var _context = _contextFactory.CreateDbContext();


        //return workouts;
        var workouts = await
        (from w in _context.Workouts
         join c in _context.Clients on w.WorkoutUserId equals c.UserId
         where c.ClientId == clientId
         select new Workout
         {
             // Client details
             WorkoutUserId = w.WorkoutUserId,
             WorkoutId = w.WorkoutId,
             WorkoutActive = w.WorkoutActive
         }).ToListAsync();


        return workouts;
    }



    
    public async Task<List<WorkoutExercise_Workout_Set_Exercise_ViewModel?>> GetWorkoutsByClientIdAndWorkoutId(int clientId, int workoutId)
    {
        using var _context = _contextFactory.CreateDbContext();

        ////return workouts;
        //var result = await
        //(from w in _context.Workouts
        // join c in _context.Clients on w.WorkoutUserId equals c.UserId
        // //join s in _context.Set on 
        // where c.ClientId == clientId
        // select new WorkoutExercise_Workout_Set_Exercise_ViewModel
        // {
        //     // Client details
        //     w = workout,

        //     WorkoutId = w.WorkoutId,
        //     WorkoutActive = w.WorkoutActive
        // }).ToListAsync();


        var workoutDetails = await (
            from e in _context.Exercises
            join we in _context.WorkoutExercises on e.ExerciseId equals we.ExerciseId
            join s in _context.Sets on we.WorkoutExerciseId equals s.WorkoutExerciseId
            join sc in _context.SetCategories on s.SetCategoryId equals sc.SetCategoryId
            where we.WorkoutId == workoutId
            orderby e.ExerciseName
            select new WorkoutExercise_Workout_Set_Exercise_ViewModel
            {
                ExerciseName = e.ExerciseName,
                MuscleGroup = e.MuscleGroup,
                Description = e.Description,
                SetId = s.SetId,
                Reps = s.Reps,
                Weight = s.Weight,
                StartTime = s.Starttime,
                EndTime = s.Endtime,
                SetCategoryType = sc.SetCategoryType,
                WorkoutExerciseId = we.WorkoutExerciseId
            }).ToListAsync();


        return workoutDetails;
    }


    public List<WorkoutExerciseModel> GetAllWorkoutsByUserId(string UserId)
    {
        {
            List<WorkoutExerciseModel> foundWorkouts = new List<WorkoutExerciseModel>();

            string sqlStatement = "SELECT * FROM workout WHERE user_id = @UserId";


            using (var connection = new NpgsqlConnection(_dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        var result = cmd.ExecuteReader();

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundWorkouts.Add(new WorkoutExerciseModel
                                {
                                    WorkoutId = (int)result["workout_id"],
                                    UserId = (int)result["user_id"],
                                    WorkoutDate = (DateTime)result["workout_date"],
                                    Duration = (TimeSpan)result["duration"],
                                    CreatedAt = (DateTime)result["created_at"]

                                });

                                //result.GetString(1));

                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");

                        }

                        result.Close();
                        //return foundClients;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

                }

                return foundWorkouts;
            }
        }
    }


    public List<WorkoutModel> GetAllWorkouts()
    {
        throw new NotImplementedException();
    }




    public List<WorkoutExercisesModel> GetWorkoutDetailsByWorkoutId(int WorkoutId)
    {
        {
            List<WorkoutExercisesModel> workoutDetails = new List<WorkoutExercisesModel>();


            string sqlStatement = @"SELECT 
                                        e.exercise_name,
                                        e.muscle_group,
                                        e.description,
                                        s.set_id,
                                        s.reps,
                                        s.weight,
                                        s.starttime,
                                        s.endtime,
                                        sc.set_category_type,
                                        we.workout_exercise_id
                                    FROM 
                                        exercise e
                                    JOIN 
                                        workout_exercise we ON e.exercise_id = we.exercise_id
                                    JOIN 
                                        set s ON we.workout_exercise_id = s.workout_exercise_id
                                    JOIN 
                                        set_category sc ON s.set_category_id = sc.set_category_id
                                    WHERE 
                                        we.workout_id = @WorkoutId
                                    ORDER BY e.exercise_name;";


            using (var connection = new NpgsqlConnection(_dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkoutId", WorkoutId);
                        var result = cmd.ExecuteReader();

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {

                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                workoutDetails.Add(new WorkoutExercisesModel
                                {
                                    ExerciseName = (string)result["exercise_name"],
                                    MuscleGroup = (string)result["muscle_group"],
                                    ExerciseDescription = (string)result["description"],
                                    Reps = (int)result["reps"],
                                    SetCategory = (string)result["set_category_type"],
                                    ExerciseGroupId = (int)result["workout_exercise_Id"],
                                    Weight = (decimal)result["weight"],
                                    SetId = (int)result["set_id"]
                                });

                                //result.GetString(1));

                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");

                        }

                        result.Close();
                        //return foundClients;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

                }

                return workoutDetails;
            }
        }
    }



    public WorkoutModel GetWorkoutById(int id)
    {
        throw new NotImplementedException();
    }

    public List<WorkoutModel> SearchWorkouts(string searchTerm)
    {
        throw new NotImplementedException();
    }


    public List<ImageModel> GetAllImagesByWeeklyReportId(int ReportId)
    {
        List<ImageModel> foundImages = new List<ImageModel>();

        string sqlStatement = @"SELECT 
                                    i.image_id,    
                                    i.file_path,    
                                    wri.weekly_report_id 
                                FROM 
                                    weekly_report_image wri 
                                JOIN image i ON wri.image_id = i.image_id
                                WHERE
                                    wri.weekly_report_id = @WeeklyReportId
                                    AND 
                                    wri.date_deleted IS NULL
                                    AND
                                    i.date_deleted IS NULL;";

        //string sqlStatement = "SELECT * FROM weekly_report WHERE date_deleted is null and user_id = @UserId";

        using (var connection = new NpgsqlConnection(_dbConnectionString))
        {

            try
            {
                connection.Open();


                using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                {
                    cmd.Parameters.AddWithValue("@WeeklyReportId", ReportId);
                    var result = cmd.ExecuteReader();

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            foundImages.Add(new ImageModel
                            {
                                ImageId = (int)result["image_id"],
                                WeeklyReportId = (int)result["weekly_report_id"],
                                ImageFilePath = result["file_path"].ToString() // Made this toString instead of at beginning
                            });
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might have occurred
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

            }
        }
        return foundImages;
    }
 


    public List<ClientWeeklyReportModel> GetAllWeeklyReportsByUserId(int UserId)
    {
        List<ClientWeeklyReportModel> foundReports = new List<ClientWeeklyReportModel>();

            // Does not return weekly_reports if the row has been deleted by the user/coach.
            string sqlStatement = "SELECT * FROM weekly_report WHERE date_deleted is null and user_id = @UserId";


            using (var connection = new NpgsqlConnection(_dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        var result = cmd.ExecuteReader();
                        //int val;

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                            //val = (int)result.GetValue(0);
                                foundReports.Add(new ClientWeeklyReportModel
                                {
                                    WeeklyReportId = (int)result["weekly_report_id"],
                                    UserId = (int)result["user_id"],
                                    ReportNotes = (string)result["notes"],
                                    CheckInDate = (DateTime)result["check_in_date"],
                                    CheckInWeight = (Decimal)result["check_in_weight"],
                                    DateCreated= (DateTime)result["date_created"],
                                    // Checks if DateDeleted contains a null value, if so assigns null value to the result instead.
                                    DateDeleted = result["date_deleted"] is DBNull ? (DateTime?)null: (DateTime)result["date_deleted"]
                                });

                                //result.GetString(1));

                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");

                        }

                        result.Close();
                        //return foundClients;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

                }

                return foundReports;
            }
        
    }



    public async Task<int> FinishUsersActiveWorkout(JsonElement WorkoutData)
    {

        // Check if WorkoutData contains the 'WorkoutData' property
        if (!WorkoutData.TryGetProperty("WorkoutData", out var setsElement))
        {
            throw new ArgumentException("ERROR: Sets provided are null...");
        }

        // Deserialize the JSON array into a list for looping through / unpacking below
        List<ReturnedSetsModel> workoutSets = JsonSerializer.Deserialize<List<ReturnedSetsModel>>(setsElement.GetRawText(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Handle case-insensitive property names
            });

        if (workoutSets == null || workoutSets.Count == 0)
        {
            return (0);
        }



        // Opens an async db connection to allow for efficient insertions/reads in the database
        await using var dataSource = NpgsqlDataSource.Create(_dbConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();


        using var batch = new NpgsqlBatch(connection);



        // Loop through each set and process it (e.g., save to database)
        foreach (var set in workoutSets)
        {
            // Example processing - print to console or handle it
            System.Diagnostics.Debug.WriteLine($"Set ID: {set.SetId}, Weight: {set.Weight}, Reps: {set.Reps}");


            batch.BatchCommands.Add(new NpgsqlBatchCommand(
                "UPDATE set " +
                "SET weight = @weight" +
                ", " +
                "reps = @reps " +
                "WHERE set_id = @set_id")

            {
                Parameters =
            {
                new NpgsqlParameter("@weight", set.Weight),
                new NpgsqlParameter("@reps", set.Reps),
                new NpgsqlParameter("@set_id", set.SetId)

            }

            });
            
        }


        try
        {
            int result = await batch.ExecuteNonQueryAsync();

            return result;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
        
        
        //    "RETURNING workout_id";
        //string sqlStatement = "UPDATE set" +
        //    "SET weight = @weight" +
        //    "SET reps = @reps" +
        //    "WHERE set_id = @set_id";


    }



        public List<WorkoutExercisesModel> GetUsersActiveWorkout(int UserId)
    {
        List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();

        // Only returns a workout if workout_active == true
        string sqlStatement = "SELECT * FROM workout WHERE workout_active and user_id = @UserId" +
            "RETURNING workout_id";


        using (var connection = new NpgsqlConnection(_dbConnectionString))
        {
            try
            {
                // Open the connection
                connection.Open();


                // Create a command object
                using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    var result = cmd.ExecuteReader();

                    System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            //val = (int)result.GetValue(0);
                            foundWorkout.Add(new WorkoutExercisesModel
                            {
                                WorkoutId = (int)result["workout_id"],
                                UserId = (int)result["user_id"],
                                WorkoutDate = (DateTime)result["workout_date"],
                                Duration = (TimeSpan)result["duration"],
                                // Checks if notes contains a null value, if so assigns null value to the result instead.
                                Notes = result["notes"] is DBNull ? (string?)null : (string)result["notes"],
                                CreatedAt = (DateTime)result["created_at"],
                                WorkoutActive = (Boolean)result["workout_active"]
                            });
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No rows found.");

                    }

                    result.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might have occurred
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

            }

            return foundWorkout;
        }

    }





    public async Task<List<int>> AddExercisesToDatabase(int WorkoutId, List<int> ExerciseIds)
    {
        List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();
        List<int> WorkoutExerciseIds = new List<int>();
        //string sqlStatement = "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')";
        int result = 0;

        // Opens an async db connection to allow for efficient insertions/reads in the database
        await using var dataSource = NpgsqlDataSource.Create(_dbConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();


        using var batch = new NpgsqlBatch(connection);

        foreach (var ExerciseId in ExerciseIds)
        {
            batch.BatchCommands.Add(new NpgsqlBatchCommand(
                "insert into workout_exercise (workout_id, exercise_id, notes, created_at) " +
                "values (@WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000') " +
                "RETURNING workout_exercise_id")

                {
                Parameters =
                {
                    new NpgsqlParameter("@WorkoutId", WorkoutId),
                    new NpgsqlParameter("@ExerciseId", ExerciseId)

                }

            });
        }

        try
        {
            await using var reader = await batch.ExecuteReaderAsync();

            do
            {
                while (await reader.ReadAsync())
                {
                    WorkoutExerciseIds.Add(reader.GetInt32(0));
                }
            } while (await reader.NextResultAsync()); // Move to the next result set

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }

        System.Diagnostics.Debug.WriteLine(WorkoutExerciseIds);
        return WorkoutExerciseIds;

    }





    public async Task<int> AddSetToDatabase(List<int> WorkoutExerciseIds)
    {
        List<WorkoutExercisesModel> workoutSets = new List<WorkoutExercisesModel>();
        //List<int> WorkoutExerciseIds = new List<int>();
        //string sqlStatement = "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')";
        int result = 0;



        // Opens an async db connection to allow for efficient insertions/reads in the database
        await using var dataSource = NpgsqlDataSource.Create(_dbConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();


        using var batch = new NpgsqlBatch(connection);

        foreach (var WorkoutExerciseId in WorkoutExerciseIds)
        {
            batch.BatchCommands.Add(new NpgsqlBatchCommand(
                "insert into set (workout_exercise_id, set_category_id, reps, weight, starttime, endtime) " +
                "values(@WorkoutExerciseId, 1, 0, 0, '10/11/2000 00:00', '10/11/2000 00:00')")
            {
            Parameters =
            {
                new NpgsqlParameter("@WorkoutExerciseId", WorkoutExerciseId),
            }
            
            });
        }

        try
        {
            result = await batch.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }


        return result;

    }






    public async Task<List<int>> CheckActiveWorkoutByUserId(string userId)
    {
        using var _context = _contextFactory.CreateDbContext();

        var activeWorkoutId = await(
                    from w in _context.Workouts
                    where w.WorkoutActive == true
                    select w.WorkoutId
                    ).ToListAsync();


        return activeWorkoutId;


        //List<WorkoutModel> foundActiveWorkout = new List<WorkoutModel>();


        //string sqlStatement = "SELECT * FROM workout WHERE UserId = @userId AND workout_active = true LIMIT 1";


        //using (var connection = new NpgsqlConnection(_dbConnectionString))
        //{
        //    try
        //    {
        //        // Open the connection
        //        connection.Open();


        //        // Create a command object
        //        using (var cmd = new NpgsqlCommand(sqlStatement, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@UserId", userId);
        //            var result = cmd.ExecuteReader();
        //            System.Diagnostics.Debug.WriteLine($"Query result: {result}");

        //            if (result.HasRows)
        //            {
        //                while (result.Read())
        //                {
        //                    //val = (int)result.GetValue(0);
        //                    foundActiveWorkout.Add(new WorkoutModel
        //                    {
        //                        WorkoutId = (int)result["workout_id"],
        //                        Duration = (TimeSpan)result["duration"],
        //                        // Checks if notes contains a null value, if so assigns null value to the result instead.
        //                        Notes = result["notes"] is DBNull ? (string?)null : (string)result["notes"]
        //                    });

        //                }
        //            }
        //            else
        //            {
        //                System.Diagnostics.Debug.WriteLine("No rows found.");

        //            }

        //            result.Close();
        //            //return foundClients;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors that might have occurred
        //        System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

        //    }

        //    System.Diagnostics.Debug.WriteLine("Returning fetch request now...");
        //    return foundActiveWorkout;
        //}

    }


    public async Task<List<WorkoutExercise_Workout_Set_Exercise_ViewModel?>> ViewActiveWorkoutByUserId(string userId)
    {
        using var _context = _contextFactory.CreateDbContext();

        var activeWorkout = await (
                    from w in _context.Workouts
                    join we in _context.WorkoutExercises on w.WorkoutId equals we.WorkoutId
                    join e in _context.Exercises on we.ExerciseId equals e.ExerciseId
                    join s in _context.Sets on we.WorkoutExerciseId equals s.WorkoutExerciseId
                    join sc in _context.SetCategories on s.SetCategoryId equals sc.SetCategoryId
                    where w.WorkoutUserId == userId && w.WorkoutActive == true
                    orderby w.WorkoutDate descending, e.ExerciseName, we.WorkoutExerciseId, s.SetId
                    select new WorkoutExercise_Workout_Set_Exercise_ViewModel
                    {
                        WorkoutId = w.WorkoutId,
                        WorkoutDate = w.WorkoutDate,
                        Duration = w.Duration,
                        ExerciseName = e.ExerciseName,
                        MuscleGroup = e.MuscleGroup,
                        Notes = we.Notes,
                        WorkoutExerciseId = we.WorkoutExerciseId,
                        SetId = s.SetId,
                        Weight = s.Weight,
                        Reps = s.Reps,
                        SetCategoryType = sc.SetCategoryType,
                        StartTime = s.Starttime,
                        EndTime = s.Endtime
                    }).ToListAsync();


        return activeWorkout;
    }


    //public List<WorkoutExercisesModel> ViewActiveWorkoutByUserId(int UserId)
    //{
    //    List<WorkoutExercisesModel> foundActiveWorkout = new List<WorkoutExercisesModel>();

    //    string sqlStatement = @"SELECT
    //                                    w.workout_id,
    //                                    w.workout_date,
    //                                    w.duration AS workout_duration,
    //                                    e.exercise_name,
    //                                    e.muscle_group,
    //                                    we.notes AS workout_exercise_notes,
    //                                 s.workout_exercise_id,
    //                                    s.set_id,
    //                                    s.weight,
    //                                 s.reps,
    //                                    sc.set_category_type AS set_category,
    //                                    s.starttime AS set_start_time,
    //                                    s.endtime AS set_end_time
    //                                FROM
    //                                    workout w
    //                                JOIN
    //                                    workout_exercise we ON we.workout_id = w.workout_id
    //                                JOIN
    //                                    exercise e ON e.exercise_id = we.exercise_id
    //                                JOIN
    //                                    set s ON s.workout_exercise_id = we.workout_exercise_id
    //                                JOIN
    //                                    set_category sc ON sc.set_category_id = s.set_category_id
    //                                WHERE
    //                                    w.user_id = @UserId
    //                                    AND w.workout_active = true
    //                                ORDER BY
    //                                    w.workout_date DESC,
    //                                    e.exercise_name,
    //                                    s.workout_exercise_id,
    //                                 s.set_id;";

    //    using (var connection = new NpgsqlConnection(_dbConnectionString))
    //    {
    //        try
    //        {
    //            // Open the connection
    //            connection.Open();


    //            // Create a command object
    //            using (var cmd = new NpgsqlCommand(sqlStatement, connection))
    //            {
    //                cmd.Parameters.AddWithValue("@UserId", UserId);
    //                var result = cmd.ExecuteReader();
    //                System.Diagnostics.Debug.WriteLine($"Query result: {result}");

    //                if (result.HasRows)
    //                {
    //                    while (result.Read())
    //                    {
    //                        //val = (int)result.GetValue(0);
    //                        foundActiveWorkout.Add(new WorkoutExercisesModel
    //                        {
    //                            ExerciseName = (string)result["exercise_name"],
    //                            WorkoutExerciseId = (int)result["workout_exercise_id"],
    //                            MuscleGroup = (string)result["muscle_group"],
    //                            WorkoutExerciseNotes = (string)result["workout_exercise_notes"],
    //                            //SetsCount = (Int64)result["sets_count"],
    //                            Reps = (int)result["reps"],
    //                            Weight = (Decimal)result["weight"],
    //                            SetId = (int)result["set_id"],
    //                            SetCategory = (string)result["set_category"]
    //                        });

    //                    }
    //                }
    //                else
    //                {
    //                    System.Diagnostics.Debug.WriteLine("No rows found.");

    //                }

    //                result.Close();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Handle any errors that might have occurred
    //            System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

    //        }

    //        System.Diagnostics.Debug.WriteLine("Returning fetch request now...");
    //        return foundActiveWorkout;
    //    }

    //}



    // public async Task<int> CreateWorkoutInDatabase(int UserId)
    // {
    //     List<WorkoutExercisesModel> foundWorkout = new List<WorkoutExercisesModel>();

    //     int result = 0;

    //     // Opens an async db connection to allow for efficient insertions/reads in the database
    //     await using var dataSource = NpgsqlDataSource.Create(_dbConnectionString);
    //     await using var connection = await dataSource.OpenConnectionAsync();


    //     using var batch = new NpgsqlBatch(connection);

    //     foreach (var ExerciseId in ExerciseIds)
    //     {
    //         batch.BatchCommands.Add(new NpgsqlBatchCommand(
    //             "insert into workout_exercise (workout_exercise_id, workout_id, exercise_id, notes, created_at) " +
    //             "values (DEFAULT, @WorkoutId, @ExerciseId, 'This has been added manually', '10/11/2022 17:50:18+0000')")
    //         {
    //             Parameters =
    //             {
    //                 new NpgsqlParameter("@WorkoutId", WorkoutId),
    //                 new NpgsqlParameter("@ExerciseId", ExerciseId)

    //             }
    //         });
    //     }

    //     try
    //     {
    //         result = await batch.ExecuteNonQueryAsync();
    //     }
    //     catch (Exception ex)
    //     {
    //         System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
    //         throw;
    //     }


    //     return result;

    //}



    public List<ExerciseModel> GetAllExercisesByUserId()
    {
        List<ExerciseModel> foundExercises = new List<ExerciseModel>();

   
        string sqlStatement = "SELECT * FROM exercise LIMIT 300";


        using (var connection = new NpgsqlConnection(_dbConnectionString))
        {
            try
            {
                // Open the connection
                connection.Open();


                // Create a command object
                using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                {
                    //cmd.Parameters.AddWithValue("@UserId", UserId);
                    var result = cmd.ExecuteReader();
                    //int val;

                    System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            //val = (int)result.GetValue(0);
                            foundExercises.Add(new ExerciseModel
                            {
                                ExerciseId = (int)result["exercise_id"],
                                ExerciseName = (string)result["exercise_name"],
                                MuscleGroup = (string)result["muscle_group"],
                                Description = (string)result["description"],
                                IsDefault = (bool)result["is_default"],
                                // Checks if user_id contains a null value, if so assigns null value to the result instead.
                                UserId = result["user_id"] is DBNull ? (int?)null : (int)result["user_id"]
                            });

                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No rows found.");

                    }

                    result.Close();
                    //return foundClients;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might have occurred
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");

            }

            System.Diagnostics.Debug.WriteLine("Returning fetch request now...");
            return foundExercises;
        }

    }



    // Takes an integer array of sets and removes them based on their set_id via a batch command
    public async Task<int> RemoveSetsFromDatabase(List<int> SetIds)
    {
        //List<WorkoutExercisesModel> workoutSets = new List<WorkoutExercisesModel>();
        int result = 0;



        // Opens an async db connection to allow for efficient insertions/reads in the database
        await using var dataSource = NpgsqlDataSource.Create(_dbConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();


        using var batch = new NpgsqlBatch(connection);

        foreach (var SetId in SetIds)
        {
            batch.BatchCommands.Add(new NpgsqlBatchCommand(
                "DELETE FROM set WHERE set_id = @SetId")
            {
                Parameters =
            {
                new NpgsqlParameter("@SetId", SetId),
            }

            });
        }

        try
        {
            result = await batch.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }


        return result;

    }
}
