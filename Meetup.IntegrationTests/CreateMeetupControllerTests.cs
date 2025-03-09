using Bogus;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Meetup.Contracts.Models;
using Xunit;

namespace Meetup.IntegrationTests;

[Collection("Test collection")]
public class CreateMeetupControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly Faker<MeetupDto> _meetupFaker;

    public CreateMeetupControllerTests(TestFixture testFixture)
    {
        _client = testFixture.HttpClient;
        _resetDatabase = testFixture.ResetDatabaseAsync;

        _meetupFaker = new Faker<MeetupDto>()
            .RuleFor(x => x.Name, faker => faker.Lorem.Word())
            .RuleFor(x => x.Organizer, faker => faker.Person.FirstName);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenMeetupCreated()
    {
        // Arrange
        var meetup = _meetupFaker.Generate();

        // Act
        var createdResponse = await _client.PostAsJsonAsync("api/meetup", meetup);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ReturnsMeetup_WhenMeetupCreated()
    {
        // Arrange
        var meetup = _meetupFaker.Generate();

        // Act
        await _client.PostAsJsonAsync("api/meetup", meetup);

        // Assert
        var createdResponse = await _client.GetAsync($"api/meetup/{meetup.Name}");
        var createdLecture = await createdResponse.Content.ReadFromJsonAsync<MeetupDetailsDto>();
        createdLecture.Name.Should().Be(meetup.Name);
        createdLecture.Organizer.Should().Be(meetup.Organizer);
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;
}
