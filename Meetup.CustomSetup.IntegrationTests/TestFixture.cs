using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hangfire;
using Moq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using MeetupAPI.Entities;
using Microsoft.Extensions.Configuration;

namespace Meetup.CustomSetup.IntegrationTests;

[CollectionDefinition(nameof(TestFixture))]
public class TestFixtureCollection : ICollectionFixture<TestFixture> { }

public class TestFixture : IAsyncLifetime
{
    public static IServiceScopeFactory BaseScopeFactory;
    private readonly TestcontainerDatabase _dbContainer = DbSetup();

    public async Task InitializeAsync()
    {
        //ToDo: add config to services
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "LocalIntegrationTesting"
        });

        await _dbContainer.StartAsync();
        builder.Configuration.GetSection("ConnectionStrings")["MeetupDb"] = _dbContainer.ConnectionString;
        await RunMigration(_dbContainer.ConnectionString);

        //builder.ConfigureServices();
        var services = builder.Services;

        // add any mock services here
        services.ReplaceServiceWithSingletonMock<IHttpContextAccessor>();

        var provider = services.BuildServiceProvider();
        BaseScopeFactory = provider.GetService<IServiceScopeFactory>();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private static async Task RunMigration(string connectionString)
    {
        var options = new DbContextOptionsBuilder<MeetupContext>()
            .UseSqlServer(connectionString)
            .Options;

        var context = new MeetupContext(options);
        await context.Database.MigrateAsync();
    }

    private static TestcontainerDatabase DbSetup()
    {
        return new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "MeetupDB",
                Username = "sa",
                Password = "CorrectHorseBatteryStapleFor$"
            })
            .WithName($"IntegrationTesting_MeetupApi_{Guid.NewGuid()}")
            .WithPortBinding(1433, true)
            .WithImage("2019-latest")
            .Build();
    }
}

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection ReplaceServiceWithSingletonMock<TService>(this IServiceCollection services)
        where TService : class
    {
        services.RemoveAll(typeof(TService));
        services.AddSingleton(_ => Mock.Of<TService>());
        return services;
    }
}
