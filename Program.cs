using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FasenmyerConference.Data;

using FasenmyerConference;
/*]]using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FasenmyerConferenceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FasenmyerConferenceContext") ?? throw new InvalidOperationException("Connection string 'FasenmyerConferenceContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.Run();*/




namespace WebApp_OpenIDConnect_DotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            /*var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<FasenmyerConferenceContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FasenmyerConferenceContext") ?? throw new InvalidOperationException("Connection string 'FasenmyerConferenceContext' not found.")));
            */
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}