using DotNet.Testcontainers.Builders;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace Meetup.IntegrationTests;

public class TestFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("db-meetup:latest")
        .WithPassword("CorrectHorseBatteryStapleFor$")
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();

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

            services.AddDbContext<MeetupContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            //services.Configure<AuthenticationOptions>(o =>
            //{
            //    if (o.Schemes is List<AuthenticationSchemeBuilder> schemes)
            //    {
            //        schemes.RemoveAll(s => s.Name == NegotiateDefaults.AuthenticationScheme);
            //        o.SchemeMap.Remove(NegotiateDefaults.AuthenticationScheme);
            //    }
            //});

            //services.AddAuthentication(defaultScheme: "TestScheme")
            //        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
            //            "TestScheme", options => { });

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
