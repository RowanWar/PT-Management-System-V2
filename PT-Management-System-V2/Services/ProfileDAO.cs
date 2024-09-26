using Npgsql;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services
{
    public class ProfileDAO : IProfileDataService
    {
        string dbConnectionString = @"Host=localhost;Username=postgres;Password=BeBetter30;Database=ptsystem;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;";

        public List<CoachModel> GetCoachProfileById(int CoachId)
        {
            List<CoachModel> foundProfile = new List<CoachModel>();

            string sqlStatement = @"SELECT 
                                    c.coach_id AS CoachId,
                                    c.coach_client_id AS CoachClientId,
                                    c.coach_profile_description AS CoachProfileDescription,
                                    c.coach_qualifications AS CoachQualifications,
                                    u.user_id AS UserId,
                                    u.email AS UserEmail,
                                    u.firstname AS FirstName,
                                    u.lastname AS LastName,
                                    u.dob AS DoB
                                FROM 
                                    coach c
                                JOIN 
                                    users u ON c.coach_client_id = u.user_id
                                WHERE 
                                    c.coach_id = @CoachId;";


            using (var connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();


                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                    {
                        cmd.Parameters.AddWithValue("@CoachId", CoachId);
                        var result = cmd.ExecuteReader();

                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                //val = (int)result.GetValue(0);
                                foundProfile.Add(new CoachModel {
                                    CoachId = (int)result["CoachId"],
                                    CoachClientId = (int)result["CoachClientId"],
                                    CoachProfileDescription = (string)result["CoachProfileDescription"],
                                    CoachQualifications = (string)result["CoachQualifications"],
                                    UserId = (int)result["UserId"],
                                    UserEmail = (string)result["UserEmail"],
                                    FirstName = (string)result["Firstname"],
                                    LastName = (string)result["LastName"],
                                    DoB = (DateTime)result["DoB"]

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
                System.Diagnostics.Debug.WriteLine(foundProfile);
                return foundProfile;
            }
        }

    }
}
