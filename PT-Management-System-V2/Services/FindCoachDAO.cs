using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services;

public class FindCoachDAO
{

    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; 


    public FindCoachDAO(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }



    public async Task<List<FindCoach_ViewModel>> GetAllCoaches(string contextUserId)
    {
        // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
        using var _context = _contextFactory.CreateDbContext();

        var coaches = await (
            from c in _context.Coaches
            join u in _context.AspNetUsers on c.UserId equals u.Id
            select new FindCoach_ViewModel
            {
                // AspNetUser Model
                CoachFirstName = u.FirstName,
                CoachLastName = u.LastName,
                CoachUserName = u.UserName,
                CoachEmail = u.Email,

                // Coach Model
                CoachProfileDescription = c.CoachProfileDescription,
                CoachQualifications = c.CoachQualifications,
            })
            .ToListAsync();


        return coaches;
    }


    //// Returns the result as to whether the currently logged in user has a (coach_id) from the coach table, if so, returns this id
    //public async Task<FindCoach_ViewModel?> DisplayCoach(string contextUserId)
    //{
    //    // Uses the factory db context to create a new instance of ApplicationDbContext on every query, which has the advantage of self-maintaining service lifetime for independency
    //    using var _context = _contextFactory.CreateDbContext();

    //    var coaches = await (
    //        from client in _context.Clients
    //        join coach in _context.Coaches on coach_client.CoachId equals coach.CoachId
    //        join coach_user in _context.AspNetUsers on coach.UserId equals coach_user.Id
    //        join client_user in _context.AspNetUsers on contextUserId equals client_user.Id
    //        where client.UserId == contextUserId
    //        select new YourCoach_ViewModel
    //        {
    //            // AspNetUsers table for the Coach
    //            CoachFirstName = coach_user.FirstName,
    //            CoachLastName = coach_user.LastName,
    //            CoachUserName = coach_user.UserName,
    //            CoachEmail = coach_user.Email,
    //            CoachPhoneNumber = coach_user.PhoneNumber,

    //            // Coach Model
    //            CoachProfileDescription = coach.CoachProfileDescription,
    //            CoachQualifications = coach.CoachQualifications,

    //            // AspNetUsers details
    //            ClientFirstName = client_user.FirstName,
    //            ClientLastName = client_user.LastName,
    //            ClientUserName = client_user.UserName,
    //            ClientEmail = client_user.Email,
    //            ClientPhoneNumber = client_user.PhoneNumber


    //        }).FirstOrDefaultAsync();

    //    return data;
    //}

}
