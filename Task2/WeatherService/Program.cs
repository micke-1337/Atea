using WeatherService.Data;
using WeatherService.Repositories;
using WeatherService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddHostedService(serviceProvider => new WeatherApiService(serviceProvider.GetRequiredService<IServiceScopeFactory>(), serviceProvider.GetRequiredService<IConfiguration>()));

builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
