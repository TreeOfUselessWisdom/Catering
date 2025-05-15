using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Catering.Data;
using Catering.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;      // for AddAuthentication()
using Microsoft.AspNetCore.Authentication.Cookies;       // if you call .AddCookie()
using Microsoft.AspNetCore.Authentication.Google;        // for GoogleDefaults & .AddGoogle()
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var pkCulture = new CultureInfo("ur-PK"); 
CultureInfo.DefaultThreadCurrentCulture = pkCulture;
CultureInfo.DefaultThreadCurrentUICulture = pkCulture;

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



builder.Services.AddSingleton<IEmailSender, LocalEmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.AddRazorPages();


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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.ItemCategories.Any())
    {
        var buffet = new ItemCategory { Name = "Buffet", Description = "Hot & cold buffet items" };
        var apps = new ItemCategory { Name = "Appetizers", Description = "Finger foods & starters" };
        var dess = new ItemCategory { Name = "Desserts", Description = "Cakes & sweets" };

        db.ItemCategories.AddRange(buffet, apps, dess);
        db.SaveChanges();

        db.Items.AddRange(
          new Item { Name = "Chicken Korma", Description = "Food", Price = 12.5m, CategoryId = buffet.CategoryId },
          new Item { Name = "Fries", Description = "Just potatoes", Price = 5.0m, CategoryId = apps.CategoryId },
          new Item { Name = "Chocolate Cake", Description = "Sweet!", Price = 6.75m, CategoryId = dess.CategoryId }
        );
        db.SaveChanges();
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


