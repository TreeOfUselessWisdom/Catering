using Microsoft.AspNetCore.Identity;
using Catering.Data;
using Catering.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;      // for AddAuthentication()
using Microsoft.AspNetCore.Authentication.Cookies;       // if you call .AddCookie()
using Microsoft.AspNetCore.Authentication.Google;        // for GoogleDefaults & .AddGoogle()
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Identity with ApplicationUser and IdentityRole
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)); // Or UseMySql for MySQL

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()            
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        // 1) request the "profile" scope so Google returns name
        googleOptions.Scope.Add("profile");

        // 2) map the JSON "name" property into ClaimTypes.Name
        googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    });





// Identity with Roles
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();




builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, LocalEmailSender>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.SeedRolesAsync(services);
    await IdentitySeed.SeedAdminAsync(services);
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed roles 
    string[] roleNames = { "Customer", "Caterer" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();


