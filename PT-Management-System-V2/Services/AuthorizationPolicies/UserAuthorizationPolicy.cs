using Microsoft.AspNetCore.Authorization;
using PT_Management_System_V2.Data;
using System.Security.Claims;

namespace PT_Management_System_V2.Services.AuthorizationPolicies;

public class UserAuthorizationPolicy : IAuthorizationRequirement
{
    public UserAuthorizationPolicy()
    {

    }
}


public class VerifyCoachAuthorizationHandler : AuthorizationHandler<UserAuthorizationPolicy>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClientDAO _clientDAO;

    public VerifyCoachAuthorizationHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ClientDAO clientDAO)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _clientDAO = clientDAO;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAuthorizationPolicy requirement)
    {
        // Grab the logged in users ID from AspNetUser (id)
        var contextUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (contextUserId == null)
        {
            context.Fail();
            return;
        }


        // Query the coach table for the coach_id where the user_id matches the logged-in user
        var coach = await _clientDAO.VerifyAndGetUsersCoachId(contextUserId);

        if (coach == null)
        {
            context.Fail();
            return;
        }
        else
        {
            context.Succeed(requirement);
        }

    }
}
