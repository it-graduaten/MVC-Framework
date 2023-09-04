using _08_01_InterimKantoor;
using _08_01_InterimKantoor.Data;
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
