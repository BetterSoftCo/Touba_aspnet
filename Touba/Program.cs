using Touba.Core;
using Utilities_aspnet.Utilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.SetupUtilities<AppDbContext>(builder.Configuration.GetConnectionString("LocalMySql"), DatabaseType.MySql);

WebApplication app = builder.Build();

app.UseUtilitiesServices();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();