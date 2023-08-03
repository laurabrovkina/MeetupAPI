using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MeetupAPI.Tests.Integration;

public class TestFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _dbContainer;
    private int Port = Random.Shared.Next(10000, 60000);

    private readonly HttpClient client;

    public TestFixture()
    {
        _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(Port, 1433)
            .WithEnvironment("Database", "MeetupDb")
            .WithCleanUp(true)
            .Build();
        client = this.CreateClient();
    }

    [Fact]
    public async void Create_CreatesMeetup_WhenDataIsValid()
    {
        // Arrange
        /*var meetup = new MeetupDto
        {
            Name = "New Meetup",
            Organizer = "Mr. Twister"
        };*/

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/meetup");

        var formModel = new Dictionary<string, string>
        {
            { "Name", "New Meetup" },
            { "Organizer", "Mr. Twister" }
        };

        postRequest.Content = new FormUrlEncodedContent(formModel);

        // Act
        var response = await client.SendAsync(postRequest);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<MeetupContext>(options => { options.UseSqlServer(_dbContainer.ConnectionString); });
            services.AddTransient<MeetupContext>();

            MvcServiceCollectionExtensions.AddMvc(services, options => options.Filters.Add(new AllowAnonymousFilter()));
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
