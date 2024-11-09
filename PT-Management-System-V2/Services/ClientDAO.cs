using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
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


    // Returns the result as to whether the currently logged in user has a (coach_id) from the coach table, and if so, returns this id
    public async Task<Coach?> VerifyAndGetUsersCoachId(string contextUserId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext(); 

        var coach = await _context.Coaches
            .FirstOrDefaultAsync(coach => coach.UserId == contextUserId);


        return coach;
    }


    public async Task<int> GetClientIdFromWorkoutId(string workoutId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var ClientId = await (
            from w in _context.Workouts
            join u in _context.AspNetUsers on w.WorkoutUserId equals u.Id
            join c in _context.Clients on w.WorkoutUserId equals c.UserId
            where w.WorkoutId == Int64.Parse(workoutId)
            select c.ClientId)
            .FirstOrDefaultAsync();


        return ClientId;
    }
    

    // Verifies user initiating a query involving a client is that users coach and therefore authorized to view data relating to them
    public async Task<bool> VerifyUserIsClientsCoach(string clientId, int coachId)
    {
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

    public async Task<List<ClientCoach_Client_ViewModel?>> GetAllClients(int contextCoachId)
    {
        using var _context = _contextFactory.CreateDbContext();

        var clients = await
        (from cc in _context.CoachClients
         join c in _context.Clients on cc.ClientId equals c.ClientId
         join wp in _context.WorkoutPrograms on c.WorkoutProgramId equals wp.WorkoutProgramId
         join u in _context.AspNetUsers on c.UserId equals u.Id

         where cc.CoachId == contextCoachId
         select new ClientCoach_Client_ViewModel
         {
             // Client details
             ClientId = c.ClientId,
             ContactByPhone = c.ContactByPhone,
             ContactByEmail = c.ContactByEmail, 
             Referred = c.Referred,
             Referral = c.Referral,
             //WorkoutProgramId = c.WorkoutProgramId,

             // WorkoutProgram details
             WorkoutProgramName = wp.ProgramName,

             // CoachClient details
             MonthlyCharge = cc.MonthlyCharge,
             ClientStartDate = cc.ClientStartDate,
             ClientEndDate = cc.ClientEndDate,

             // AspNetUsers details
             FirstName = u.FirstName,
             LastName = u.LastName,
             UserName = u.UserName,
             Email = u.Email,
             PhoneNumber = u.PhoneNumber
         }).ToListAsync();


        return clients;
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
