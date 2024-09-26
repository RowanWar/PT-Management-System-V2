//using microsoft.data.sqlclient;
//using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Npgsql;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.services
{
    public class SecurityDAO
    {
        //string sqlStatement = "select * from users WHERE username = @username AND password = @password";     


        public static bool FindUserByNameAndPassword(UserLogin user)
        {
            bool success = false;
            string connectionString = "Host=localhost;Username=postgres;Password=BeBetter30;Database=ptsystem;Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    conn.Open();

                    // Define a simple query
                    //string sqlString = "SELECT * FROM users";
                    string sqlStatement = "select * from users WHERE username = @username AND password = @password";

                    // Create a command object
                    using (var cmd = new NpgsqlCommand(sqlStatement, conn))
                    {
                        cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Varchar, 50).Value = user.UserName;
                        cmd.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar, 50).Value = user.Password;
                        var result = cmd.ExecuteReader();
                        //Console.WriteLine($"Query result: {result}");
                        System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                        if (result.HasRows)
                        {
                            success = true;
                            while (result.Read())
                            {
                                System.Diagnostics.Debug.WriteLine("{0}\t{1}", result.GetInt32(0),
                                    result.GetString(1));
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No rows found.");

                        }

                        result.Close();
                        return success;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might have occurred
                    System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
                    return success;
                }
            }

        }
    }
}
