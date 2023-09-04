using _09_01_InterimKantoor.Models;
using _09_01_InterimKantoor;
using _09_01_InterimKantoor.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<InterimKantoorContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));
builder.Services.AddRazorPages();
// Registratie voor dependency injection van UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Swagger registreren
builder.Services.AddSwaggerGen();

//NewtonJSonSoft registreren
builder.Services.AddControllersWithViews().AddNewtonsoftJson(Options => Options
    .SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDefaultIdentity<CustomUser>().AddEntityFrameworkStores<InterimKantoorContext>();

builder.Services.Configure<IdentityOptions>(options => 
{ 
    // Password settings.
    options.Password.RequireDigit = true; 
    options.Password.RequireLowercase = true; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = true; 
    options.Password.RequiredLength = 8; 
    options.Password.RequiredUniqueChars = 1; 

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); 
    options.Lockout.MaxFailedAccessAttempts = 3; 
    options.Lockout.AllowedForNewUsers = true; 
    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789-._@+";
    options.User.RequireUniqueEmail = false; });


var app = builder.Build();

// Swagger middleware koppelen aan app
app.UseSwagger();

// SwaggerUI instellen met juiste JSON endpoint 
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
