using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Infrastructure.Authentication;
using PT_Management_System_V2.Services;
using PT_Management_System_V2.Services.AuthorizationPolicies;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Grabs the signing key data from appsettings.json (should be moved for production) for use to validate the JWT when authing to protected JWT api endpoints
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var connectionString = builder.Configuration["ConnectionStrings:PtSystemDb"] ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//void ConfigureDbContextOptions(DbContextOptionsBuilder options)
//{
//    options.UseNpgsql(connectionString); // Use your actual database provider here
//}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString),
    optionsLifetime: ServiceLifetime.Singleton);


builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString), 
    ServiceLifetime.Singleton);


//builder.Services.AddSingleton<WorkoutDAO>(provider => new WorkoutDAO(connectionString));


// Registers as a singleton whilst using DI to safely manage the lifetime of ContextFactory
builder.Services.AddSingleton<WorkoutDAO>(provider =>
{
    var contextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    return new WorkoutDAO(connectionString, contextFactory);
});

builder.Services.AddSingleton<ClientDAO>(provider =>
    {
        var contextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            return new ClientDAO(connectionString, contextFactory); 
    });

builder.Services.AddSingleton<ReportDAO>(provider =>
{
    var contextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    return new ReportDAO(contextFactory);
});

builder.Services.AddSingleton<YourCoachDAO>(provider =>
{
    var contextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    return new YourCoachDAO(contextFactory);
});


//builder.Services.AddSingleton<YourCoachDAO>(provider =>
//{
//    var contextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
//    return new YourCoachDAO(contextFactory);
//});

// Service dedicated to handling the generation of JWT tokens after successful user login/authentication
builder.Services.AddScoped<JwtTokenService>();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    // Default authentication scheme is set to cookies only for user account logins
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"], 
        ValidAudience = jwtSettings["Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])) 
    };


    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var error = context.Exception;
            // Log or inspect the error here
            System.Diagnostics.Debug.WriteLine($"JWT Authentication failed: {error.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // If token is invalid, OnChallenge will be triggered
            System.Diagnostics.Debug.WriteLine("JWT Bearer token is invalid or missing.");
            return Task.CompletedTask;
        }
    };
});

// Added last night
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CoachPolicy", policy =>
        policy.Requirements.Add(new CoachAuthorizationPolicy()));
});

// Imports the authorization policy responsible for protecting controller endpoints to only authorised coaches from viewing client data
builder.Services.AddScoped<IAuthorizationHandler, CoachAuthorizationHandler>();

builder.Services.AddHttpContextAccessor(); // Needed to access route data

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();



builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnSigningIn = async context =>
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var jwtTokenService = context.HttpContext.RequestServices.GetRequiredService<JwtTokenService>();

        // Get the user context of the signed in user
        var user = await userManager.GetUserAsync(context.Principal);
        System.Diagnostics.Debug.WriteLine(user.Id);
        // Generate JWT token
        var token = await jwtTokenService.GenerateToken(user);

        // Adds the generated JWT as a response header to the login response
        context.HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

        await Task.CompletedTask;
    };
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
