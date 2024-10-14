using Microsoft.EntityFrameworkCore;
using Npgsql;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using PT_Management_System_V2.Data.ViewModels;
using PT_Management_System_V2.Models;

namespace PT_Management_System_V2.Services;

public class ReportDAO /*: IClientDataService*/
{

    private readonly string _dbConnectionString;
    // Uses a DbContextFactory to create new instances of dbcontext for scoped/transient background services (such as this), which is best-practise rather than relying on IServiceProvider which is an anti-pattern.
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;


    public ReportDAO(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    public async Task<List<WeeklyReport?>> GetAllWeeklyReportsOfUser(int client_id)
    {
        using var _context = _contextFactory.CreateDbContext();

        var reports = await
        (from wr in _context.WeeklyReports
         //join c in _context.Clients on cc.ClientId equals c.ClientId
         //join u in _context.AspNetUsers on c.UserId equals u.Id
         where wr.ClientId == client_id
         select new WeeklyReport
         {
             // Client details
             WeeklyReportId = wr.WeeklyReportId,
             Notes = wr.Notes,
             CheckInDate = wr.CheckInDate,
             CheckInWeight = wr.CheckInWeight,
             ClientId = wr.ClientId
         }).ToListAsync();


        return reports;
    }

}
