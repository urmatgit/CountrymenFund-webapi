using FSH.WebApi.Infrastructure.Identity;
using FSH.WebApi.Infrastructure.Persistence.Context;

using FSH.WebApi.Application;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Respawn.Graph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Models;

using Finbuckle.MultiTenant;
using FSH.WebApi.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Application.IntegrationTests.Configurations;
using FSH.WebApi.Infrastructure.Persistence;
using FSH.WebApi.Infrastructure.Common.Services;
using FSH.WebApi.Application.Common.Events;
using FSH.WebApi.Infrastructure;

namespace Application.IntegrationTests;
[SetUpFixture]
public  class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Respawner _checkpoint;
    private static string _currentUserId;
    private static string _currentTenantId="root";
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddConfigurations()
           // .RegisterSerilog()
            .AddEnvironmentVariables();
        
        _configuration = builder.Build();

        //var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(_configuration);
        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "FSH.WebApi.Host"));
        services.AddInfrastructure(_configuration)
        //services.AddOptions<DatabaseSettings>()
        //   .BindConfiguration(nameof(DatabaseSettings))
        //   .PostConfigure(databaseSettings =>
        //   {
        //       Console.WriteLine("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
        //   })
        //   .ValidateDataAnnotations()
        //   .ValidateOnStart()
           
        //   ;
           
        //services.AddDbContext<ApplicationDbContext>((p, m) =>
        //{
        //    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
        //    m.UseSqlServer(databaseSettings.ConnectionString, e =>
        //                         e.MigrationsAssembly("Migrators.MSSQL"));
        //    //m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
        //}).AddLogging()
        
                .AddApplication();

        //services.AddLogging();

        //startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUser));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddScoped(provider =>
            Mock.Of<ICurrentUser>(s => true));//   s.GetUserId().ToString() == _currentUserId));
        services.AddScoped<ITenantInfo,TenantInfo>(provider=>
        new TenantInfo() { Name="root",ConnectionString= _configuration.GetConnectionString("ConnectionString") });
        services.AddTransient<ISerializerService, NewtonSoftService>();
        services.AddTransient<IEventPublisher, EventPublisher>();
        
        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = await Respawner.CreateAsync(_configuration.GetConnectionString("ConnectionString"), new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
        });

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();
        
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("Demo", "123Pa$$word!", new string[] { });
    }

    public static async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("admin", "123Pa$$word!", new[] { "Admin" });
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            _currentUserId = user.Id;

            return _currentUserId;
        }

        var errors = string.Join(Environment.NewLine, result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description)).Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public static async Task ResetState()
    {
        await _checkpoint.ResetAsync(_configuration.GetConnectionString("DefaultConnection"));
        _currentUserId = null;
        _currentTenantId = null;
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    //public static IPicklistService CreatePicklistService()
    //{
    //    var scope = _scopeFactory.CreateScope();
    //    return scope.ServiceProvider.GetRequiredService<IPicklistService>();
    //}
    //public static ITenantsService CreateTenantsService()
    //{
    //    var scope = _scopeFactory.CreateScope();
    //    return scope.ServiceProvider.GetRequiredService<ITenantsService>();
    //}

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
