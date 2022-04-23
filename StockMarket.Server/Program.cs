using StockMarket.Server.Data;
using StockMarket.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockMarket.Shared.Data.Context;
using StockMarket.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using StockMarket.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var stockMarketConnection = builder.Configuration.GetConnectionString("StockMarketConnection");


// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<StockMarketContext>(options =>
    options.UseSqlServer(stockMarketConnection));

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        //Note: Microsoft recommends to NOT migrate your database at Startup. 
        //You should consider your migration strategy according to the guidelines.
        serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    }

    using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        var user = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@abc.com"
        };

        var service = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var euser = service.FindByNameAsync(user.UserName).Result;

        if (euser == null)
        {
            var res = service.CreateAsync(user, "Admin@123").Result;
            var addedUser = service.FindByNameAsync(user.UserName).Result;
            var ress = service.AddToRoleAsync(addedUser, "admin").Result;
        }


    }

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.MapHub<MarketHub>("/markethub");
app.MapHub<MarketHub>("/portfoliohub");

app.MapFallbackToFile("index.html");

var hubContext = app.Services.GetService(typeof(IHubContext<MarketHub>));

StockMarketService.HubContext = hubContext as IHubContext<MarketHub>;

var phubContext = app.Services.GetService(typeof(IHubContext<PortfolioHub>));
StockMarketService.portfolioHubContext = phubContext as IHubContext<PortfolioHub>;

StockMarketService.Instance.Initialize();

app.Run();
