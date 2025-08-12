using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.MsSql;
using Xunit;

namespace Meetup.IntegrationTests;

public class TestFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string Database = "MeetupDB";
    private const string Username = "sa";
    private const string Password = "CorrectHorseBatteryStapleFor$";
    private const ushort MsSqlPort = 1433;

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("db-meetup:latest")
        .WithPortBinding(MsSqlPort, true)
        .WithPassword(Password)
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlPort))
        .Build();

    private DbConnection _dbConnection;
    private Respawner _respawner;

    public HttpClient HttpClient { get; private set; } 

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descrtiptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<MeetupContext>));

            if (descrtiptor is not null)
            {
                services.Remove(descrtiptor);
            }

            var host = _dbContainer.Hostname;
            var port = _dbContainer.GetMappedPublicPort(MsSqlPort);

            services.AddDbContext<MeetupContext>(options =>
            {
                options.UseSqlServer(
                    $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True");
            });

            Thread.Sleep(90000);

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        HttpClient = CreateClient();
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
