using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services;

public class YourCoachDAO
{

    private readonly string _dbConnectionString;
    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; 


    public YourCoachDAO(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    // Returns the result as to whether the currently logged in user has a (coach_id) from the coach table, if so, returns this id
    public async Task<List<YourCoach_ViewModel?>> DisplayCoach(string contextUserId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var data = await (
            from client in _context.Clients
            join coach_client in _context.CoachClients on client.ClientId equals coach_client.ClientId
            join coach in _context.Coaches on coach_client.CoachId equals coach.CoachId
            join coach_user in _context.AspNetUsers on coach.UserId equals coach_user.Id
            join client_user in _context.AspNetUsers on contextUserId equals client_user.Id
            where client.UserId == contextUserId
            select new YourCoach_ViewModel
            {
                // AspNetUsers table for the Coach
                CoachFirstName = coach_user.FirstName,
                CoachLastName = coach_user.LastName,
                CoachUserName = coach_user.UserName,
                CoachEmail = coach_user.Email,
                CoachPhoneNumber = coach_user.PhoneNumber,

                // CoachClient details
                MonthlyCharge = coach_client.MonthlyCharge,
                StartDate = coach_client.ClientStartDate,
                EndDate = coach_client.ClientEndDate,

                // AspNetUsers details
                ClientFirstName = client_user.FirstName,
                ClientLastName = client_user.LastName,
                ClientUserName = client_user.UserName,
                ClientEmail = client_user.Email,
                ClientPhoneNumber = client_user.PhoneNumber
            }).ToListAsync();

        return data;
    }


    // Coach Model 
    public string? CoachProfileDescription { get; set; }

    public string? CoachQualifications { get; set; }


    // Client Model
    public string? ClientFirstName { get; set; }

    public string? ClientLastName { get; set; }

    public string? ClientUserName { get; set; }

    public string? ClientEmail { get; set; }

    public string? ClientPhoneNumber { get; set; }




    //public async Task<bool> VerifyUserIsClientsCoach(string clientId, int coachId)
    //{
    //    using var _context = _contextFactory.CreateDbContext();

    //    // Join with the coach_client table and check if the (coach_id) matches the (client_id) being queried
    //    var isMatch = await _context.CoachClients
    //        .AnyAsync(coacli => coacli.CoachId == coachId && coacli.ClientId == int.Parse(clientId));


    //    return isMatch;
    //}


}
