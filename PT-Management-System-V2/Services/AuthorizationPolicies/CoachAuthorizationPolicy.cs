using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Data.EntityFrameworkModels;
using System.Security.Claims;

namespace PT_Management_System_V2.Services.AuthorizationPolicies;


// Handler for verifying the logged in user is the coach of the client
public class CoachAuthorizationPolicy : IAuthorizationRequirement
{
    public CoachAuthorizationPolicy() 
    { 
    }
}

public class CoachAuthorizationHandler : AuthorizationHandler<CoachAuthorizationPolicy> 
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClientDAO _clientDAO;

    public CoachAuthorizationHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ClientDAO clientDAO)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _clientDAO = clientDAO;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CoachAuthorizationPolicy requirement)
    {
        // Grab the logged in users ID from AspNetUser (id)
        var contextUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (contextUserId == null)
        {
            context.Fail();
            return;
        }

        // Grab the client_id being queried via queryParameter 
        var clientId = _httpContextAccessor.HttpContext.Request.Query["client_id"].ToString();


        // If no queryParameter passed of client_id > fail authorization
        if (string.IsNullOrEmpty(clientId))
        {
            context.Fail();
            return;
        }

        // Query the coach table for the coach_id where the user_id matches the logged-in user
        var coach = await _clientDAO.VerifyUsersCoachId(contextUserId);

        if (coach == null)
        {
            context.Fail();
            return;
        }
 

        // Join with the coach_client table and check if the (coach_id) matches the (client_id) being queried
        // Returns a true/false boolean as to whether the user initiating the query is the clients coach
        var isMatch = await _clientDAO.VerifyUserIsClientsCoach(clientId, coach.CoachId);

        if (isMatch)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        //var verifyIsClientsCoach = await _context.Coaches
        //    .Select(coach => coach.UserId).Where()
        //    //.Where(coach.User)
        //    .AnyAsync();
        // get coach id based on user id
        // if empty, then doesn't exist
    }
}
