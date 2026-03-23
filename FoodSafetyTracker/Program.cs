using FoodSafetyTracker.Data;
using FoodSafetyTracker.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithProperty("Application", "FoodSafetyTracker")
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Application} {EnvironmentName} {UserName} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting FoodSafetyTracker application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.Use(async (context, next) =>
    {
        using (LogContext.PushProperty("UserName", context.User?.Identity?.Name ?? "anonymous"))
        {
            await next();
        }
    });
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var role in new[] { "Admin", "Inspector", "Viewer" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Log.Information("Role created: {Role}", role);
            }
        }

        var adminEmail = "admin@foodsafety.ie";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                Log.Information("Default admin user created: {Email}", adminEmail);
            }
        }

        var inspectorEmail = "inspector@foodsafety.ie";
        if (await userManager.FindByEmailAsync(inspectorEmail) == null)
        {
            var inspector = new IdentityUser { UserName = inspectorEmail, Email = inspectorEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(inspector, "Inspector123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(inspector, "Inspector");
                Log.Information("Default inspector user created: {Email}", inspectorEmail);
            }
        }

        var viewerEmail = "viewer@foodsafety.ie";
        if (await userManager.FindByEmailAsync(viewerEmail) == null)
        {
            var viewer = new IdentityUser { UserName = viewerEmail, Email = viewerEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(viewer, "Viewer123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(viewer, "Viewer");
                Log.Information("Default viewer user created: {Email}", viewerEmail);
            }
        }
    }

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
