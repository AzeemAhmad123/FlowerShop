using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // ADD THIS for Identity services
using FlowerShop.Data;
using FlowerShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure anti-forgery to work with AJAX requests (token in header)
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
});

// Add session support for shopping cart
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework Core DbContext
builder.Services.AddDbContext<FlowerShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- ADD THIS BLOCK FOR CLO4 SECURITY (IDENTITY) ---

// Register Identity services, linking them to your FlowerShopContext and supporting roles.
builder.Services.AddIdentity<Client, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<FlowerShopContext>()
    .AddDefaultTokenProviders();

// ----------------------------------------------------

var app = builder.Build();

// Seed roles on application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        RoleSeeder.SeedRolesAsync(services).GetAwaiter().GetResult();

        // TEMPORARY: Create admin user (run once, then remove this code)
        var userManager = services.GetRequiredService<UserManager<Client>>();
        var adminEmail = "admin@flowershop.com";
        var adminPassword = "Admin123!";
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            adminUser = new Client
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                Gender = "Other",
                Age = 30
            };
            var createResult = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
            if (createResult.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add security headers middleware
app.Use(async (context, next) =>
{
    // XSS Protection
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

    // Prevent MIME type sniffing
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // Clickjacking protection
    context.Response.Headers.Append("X-Frame-Options", "DENY");

    // Referrer Policy
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

    // Content Security Policy (basic - adjust as needed)
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com https://fonts.googleapis.com; " +
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com; " +
        "font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; " +
        "img-src 'self' data: https:; " +
        "connect-src 'self';");

    await next();
});

app.UseSession(); // Enable session middleware
app.UseStaticFiles();

app.UseRouting();

// CRITICAL: Ensure Authentication runs BEFORE Authorization
app.UseAuthentication(); // ADDED - Checks who the user is
app.UseAuthorization(); // Confirms user's permissions

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();