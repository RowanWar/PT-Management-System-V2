using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PT_Management_System_V2.Data;
using PT_Management_System_V2.Infrastructure.Authentication;
using PT_Management_System_V2.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:PtSystemDb"] ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<WorkoutDAO>(provider => new WorkoutDAO(connectionString));
builder.Services.AddSingleton<ClientDAO>(provider => new ClientDAO(connectionString));
builder.Services.AddScoped<JwtTokenService>();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add JWT Authentication
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
        ValidIssuer = "Issuer", 
        ValidAudience = "Audience", 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SigningKey"))
    };
});



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
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var jwtTokenService = context.HttpContext.RequestServices.GetRequiredService<JwtTokenService>();

        // Get the user
        var user = await userManager.GetUserAsync(context.Principal);

        // Generate JWT token
        var token = await jwtTokenService.GenerateToken(user);

        // Here, you could add the token to the response header or body as per your use case
        context.HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

        await Task.CompletedTask;
    };
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

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
//app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
