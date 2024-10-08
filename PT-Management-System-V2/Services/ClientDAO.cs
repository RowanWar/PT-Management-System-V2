using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services;

public class ClientDAO : IClientDataService
{

    private readonly string _dbConnectionString;
    private readonly ApplicationDbContext _context;

    public ClientDAO(string dbConnectionString, ApplicationDbContext context)
    {
        _dbConnectionString = dbConnectionString;
        _context = context;
    }


    public int Delete(ClientModel client)
    {
        throw new NotImplementedException();
    }

    public List<ClientModel> GetAllClients()
    {
        List<ClientModel> foundClients = new List<ClientModel>();

        string sqlStatement = "SELECT * FROM users";


        using (var connection = new NpgsqlConnection(_dbConnectionString))
        {
            try
            {
                // Open the connection
                connection.Open();


                // Create a command object
                using (var cmd = new NpgsqlCommand(sqlStatement, connection))
                {
                    var result = cmd.ExecuteReader();
                    //int val;

                    System.Diagnostics.Debug.WriteLine($"Query result: {result}");

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            //val = (int)result.GetValue(0);
                            foundClients.Add(new ClientModel { 
                                ClientUserId = (int)result["user_id"],
                                ClientUsername = (string)result["username"],
                                ClientFirstName = (string)result["firstname"], 
                                ClientLastName= (string)result["lastname"],
                                ClientEmail = (string)result["email"],
                                ClientPhone = (string)result["mobile_number"]
                     
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

            return foundClients;
        }
    }

    public ClientModel GetClientById(int id)
    {
        throw new NotImplementedException();
    }

    public int Insert(ClientModel client)
    {
        throw new NotImplementedException();
    }

    public List<WorkoutExerciseModel> SearchClients(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public int Update(ClientModel client)
    {
        throw new NotImplementedException();
    }

    List<ClientModel> IClientDataService.GetAllClients()
    {
        throw new NotImplementedException();
    }

    List<ClientModel> IClientDataService.SearchClients(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public List<DataModel> GetCoachesClients()
    {
        var clientNames = _context.Users
        .Select(u => u.Username)
        .ToList();
    }
}
