using Bogus;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Mappings;
using Xunit;

namespace Meetup.IntegrationTests;

[Collection("Test collection")]
public class CreateLectureControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly Faker<MeetupResponse> _meetupFaker;
    private readonly Faker<LectureDto> _lectureFaker;

    public CreateLectureControllerTests(TestFixture testFixture)
    {
        _client = testFixture.HttpClient;
        _resetDatabase = testFixture.ResetDatabaseAsync;

        _meetupFaker = new Faker<MeetupResponse>()
            .RuleFor(x => x.Name, faker => faker.Lorem.Word())
            .RuleFor(x => x.Organizer, faker => faker.Person.FirstName);
        _lectureFaker = new Faker<LectureDto>()
            .RuleFor(x => x.Author, faker => faker.Person.FullName)
            .RuleFor(x => x.Topic, faker => faker.Lorem.Word())
            .RuleFor(x => x.Description, faker => faker.Lorem.Sentence());
    }

    // This test will be skipped only on environment with name set up to "staging"
    // Look at the deployment.yaml for more details
    [CustomFact]
    // Or use `BeforeAfter` to set environment to particular stage for this test
    // [BeforeAfter]
    public async Task Create_ReturnsCreated_WhenLectureCreated()
    {
        // Arrange
        var meetup = _meetupFaker.Generate();

        var createResponse = await _client.PostAsJsonAsync("api/meetup", meetup);
        createResponse.EnsureSuccessStatusCode();

        var lecture = _lectureFaker.Generate();

        // Act
        var response = await _client.PostAsJsonAsync($"api/meetup/{meetup.Name}/lecture", lecture);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ReturnsLecture_WhenLectureCreated()
    {
        // Arrange
        var meetup = _meetupFaker.Generate();

        var createMeetup = await _client.PostAsJsonAsync("api/meetup", meetup);
        createMeetup.EnsureSuccessStatusCode();

        var lecture = _lectureFaker.Generate();

        // Act
        await _client.PostAsJsonAsync($"api/meetup/{meetup.Name}/lecture", lecture);

        // Assert
        var createdResponse = await _client.GetAsync($"api/meetup/{meetup.Name}/lecture");
        var createdLecture = await createdResponse.Content.ReadFromJsonAsync<List<LectureDto>>();
        // if this assertion is skipped, in case 'createdLecture' is null,
        // the whole test will pass even so the collection is null
        // and the test has to fail in this case
        createdLecture.Should().NotBeNull();
        createdLecture?.First()?.Author.Should().Be(lecture.Author);
        createdLecture?.First()?.Topic.Should().Be(lecture.Topic);
        createdLecture?.First()?.Description.Should().Be(lecture.Description);
    }

    public Task DisposeAsync() => _resetDatabase();

    public Task InitializeAsync() => Task.CompletedTask;
}
