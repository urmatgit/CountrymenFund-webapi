using FSH.WebApi.Application;
using FSH.WebApi.Host.Configurations;
using FSH.WebApi.Host.Controllers;
using FSH.WebApi.Infrastructure;
using FSH.WebApi.Infrastructure.Common;

using FSH.WebApi.Infrastructure.Logging.Serilog;
using Serilog;
using Serilog.Formatting.Compact;

using Microsoft.AspNetCore.HttpOverrides;

using System.Net;


[assembly: ApiConventionType(typeof(FSHApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        if (!builder.Environment.IsDevelopment())
            options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
        //options.ForwardedHeaders = ForwardedHeaders.All;
        //options.KnownNetworks.Clear();
        //options.KnownProxies.Clear();
    });
    var app = builder.Build();
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }
    await app.Services.InitializeDatabasesAsync();
    //app.UseForwardedHeaders(new ForwardedHeadersOptions
    //{
    //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    //});
    app.UseInfrastructure(builder.Configuration);
    
    app.MapEndpoints();
    
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}