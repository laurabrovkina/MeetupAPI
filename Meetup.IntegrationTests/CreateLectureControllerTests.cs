using FluentAssertions;
using MeetupAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Meetup.IntegrationTests;

public class CreateLectureControllerTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;

    public CreateLectureControllerTests(TestFixture testFixture)
    {
        _client = testFixture.CreateClient();
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenLectureCreated()
    {
        // Arrange
        var meetup = new MeetupDto
        {
            Name = "meetup",
            Organizer = "Nick",
            Date = DateTime.Now,
            IsPrivate = false
        };

        var createResponse = await _client.PostAsJsonAsync("api/meetup", meetup);
        createResponse.EnsureSuccessStatusCode();

        var lecture = new LectureDto
        {
            Author = "New author",
            Topic = "topic",
            Description = "description"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"api/meetup/{meetup.Name}/lecture", lecture);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ReturnsLecutre_WhenLectureCreated()
    {
        // Arrange
        var meetup = new MeetupDto
        {
            Name = "new-meetup",
            Organizer = "Nick"
        };

        var createMeetup = await _client.PostAsJsonAsync("api/meetup", meetup);
        createMeetup.EnsureSuccessStatusCode();

        var lecture = new LectureDto
        {
            Author = "New author",
            Topic = "topic",
            Description = "description"
        };

        // Act
        await _client.PostAsJsonAsync($"api/meetup/{meetup.Name}/lecture", lecture);

        // Assert
        var createdResponse = await _client.GetAsync($"api/meetup/{meetup.Name}/lecture");
        var createdLecture = await createdResponse.Content.ReadFromJsonAsync<List<LectureDto>>();
        createdLecture.FirstOrDefault().Author.Should().Be(lecture.Author);
        createdLecture.FirstOrDefault().Topic.Should().Be(lecture.Topic);
        createdLecture.FirstOrDefault().Description.Should().Be(lecture.Description);
    }
}
