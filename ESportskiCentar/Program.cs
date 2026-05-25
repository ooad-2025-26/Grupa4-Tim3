using ESportskiCentar.Data;
using ESportskiCentar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CONNECTION STRING
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nije pronađen.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// IDENTITY + ROLE
builder.Services.AddDefaultIdentity<Korisnik>(options =>
{
    // EMAIL CONFIRMATION OFF
    options.SignIn.RequireConfirmedAccount = false;

    // PASSWORD SETTINGS
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();


// AUTOMATSKO KREIRANJE ROLA I ADMINA
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Korisnik>>();

    // ROLE
    string[] roles =
    {
        RoleNames.Administrator,
        RoleNames.Radnik,
        RoleNames.Korisnik
    };

    foreach (var role in roles)
    {
        bool postoji = await roleManager.RoleExistsAsync(role);

        if (!postoji)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ADMIN
    string adminEmail = "admin@centar.ba";
    string adminPassword = "admin123";

    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        var noviAdmin = new Korisnik
        {
            UserName = adminEmail,
            Email = adminEmail,
            ime = "Admin",
            prezime = "Vlasnik",
            EmailConfirmed = true
        };

        var rezultat = await userManager.CreateAsync(noviAdmin, adminPassword);

        // AKO JE USPJEŠNO KREIRAN
        if (rezultat.Succeeded)
        {
            await userManager.AddToRoleAsync(noviAdmin, RoleNames.Administrator);
        }
    }
}


// CONFIGURE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// AUTH
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// ROUTES
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();