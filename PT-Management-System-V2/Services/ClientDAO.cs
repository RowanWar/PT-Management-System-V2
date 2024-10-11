using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services;

public class ClientDAO : IClientDataService
{

    private readonly string _dbConnectionString;
    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; 


    public ClientDAO(string dbConnectionString, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _dbConnectionString = dbConnectionString;
        _contextFactory = contextFactory;
    }


    // Returns the result as to whether the currently logged in user has a (coach_id) from the coach table, if so, returns this id
    public async Task<Coach?> VerifyUsersCoachId(string contextUserId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext(); // Does this need to be wrapped in { } so it automatically closes itself?

        var coach = await _context.Coaches
            .FirstOrDefaultAsync(coach => coach.UserId == contextUserId);

        return coach;
    }

    public async Task<bool> VerifyUserIsClientsCoach(string clientId, int coachId)
    {
        //int coachIdInt = int.TryParse(coachId, out intOut);
        using var _context = _contextFactory.CreateDbContext();
        // Join with the coach_client table and check if the (coach_id) matches the (client_id) being queried
        var isMatch = await _context.CoachClients
            .AnyAsync(coacli => coacli.CoachId == coachId && coacli.ClientId == int.Parse(clientId));

        return isMatch;
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
}
