using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Match.Business.DependencyResolvers.Autofac;
using Match.Core.DependencyResolvers;
using Match.Core.Utilities.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Match.Core.Extensions;
using Serilog;
using Serilog.Events;
using Match.Web.Exceptions;

var builder = WebApplication.CreateBuilder(args);
var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here.
// Don't call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(
builder => builder.RegisterModule(new AutofacBusinessModule()));
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/login";
    option.AccessDeniedPath = "/accesdenied";

    option.SlidingExpiration = true;
    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
#region  serilog service registration


Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

Log.Logger = new LoggerConfiguration()
.Enrich.WithProperty("ApplicationContext", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
.Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
.Enrich.FromLogContext()
.Enrich.WithClientIp()
.Enrich.WithRequestHeader(headerName: "User-Agent")
.WriteTo.Console()
.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
.MinimumLevel.Information()
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();
#endregion

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

