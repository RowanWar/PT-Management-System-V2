//using Microsoft.EntityFrameworkCore;
//using Npgsql;
//using PT_Management_System_V2.Data;
//using PT_Management_System_V2.Data.EntityFrameworkModels;
//using PT_Management_System_V2.Data.ViewModels;
//using PT_Management_System_V2.Models;

//namespace PT_Management_System_V2.Services;

//public class ReportDAO /*: IClientDataService*/
//{

//    private readonly string _dbConnectionString;
//    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
//    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


//    public ReportDAO(string dbConnectionString, IDbContextFactory<ApplicationDbContext> contextFactory)
//    {
//        _dbConnectionString = dbConnectionString;
//        _contextFactory = contextFactory;
//    }


//    public async Task<List<ClientCoach_ClientViewModel?>> GetAllUserWeeklyReports(int UserId)
//    {
//        using var _context = _contextFactory.CreateDbContext();

//        var reports = await
//        (from cc in _context.CoachClients
//         join c in _context.Clients on cc.ClientId equals c.ClientId
//         join u in _context.AspNetUsers on c.UserId equals u.Id
//         where cc.CoachId == contextCoachId
//         select new ClientCoach_ClientViewModel
//         {
//             // Client details
//             ClientId = c.ClientId,
//             ContactByPhone = c.ContactByPhone,
//             ContactByEmail = c.ContactByEmail,
//             Referred = c.Referred,
//             Referral = c.Referral,

//             // CoachClient details
//             MonthlyCharge = cc.MonthlyCharge,
//             ClientStartDate = cc.ClientStartDate,
//             ClientEndDate = cc.ClientEndDate,

//             // AspNetUsers details
//             FirstName = u.FirstName,
//             LastName = u.LastName,
//             UserName = u.UserName,
//             Email = u.Email,
//             PhoneNumber = u.PhoneNumber
//         }).ToListAsync();


//        return clients;
//    }

//}
