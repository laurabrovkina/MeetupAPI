using FluentAssertions;
using MeetupAPI.Entities;
using MeetupAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        //_client.DefaultRequestHeaders.Authorization =
        //    new AuthenticationHeaderValue(scheme: "TestScheme");
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenLectureCreated()
    {
        // Arrange
        // Create roles when db is up in container
        //INSERT INTO
        //[MeetupDb].[dbo].[Roles]
        //VALUES('User'),('Moderator'),('Admin')

        var user = new RegisterUserDto
        {
            Email = "test@test.com",
            RoleId = 3 // Admin
        };

        var createUser= await _client.PostAsJsonAsync("api/account/register", user);
        createUser.EnsureSuccessStatusCode();

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
            Name = "meetup",
            Organizer = "Nick"
        };

        var lecture = new LectureDto
        {
            Author = "New author",
            Topic = "topic",
            Description = "description"
        };

        var createResponse = await _client.PostAsJsonAsync($"api/meetup/{meetup.Name}/lecture", lecture);
        var createdLecture = await createResponse.Content.ReadFromJsonAsync<Lecture>();

        // Act
        createdLecture.Author.Should().Be(lecture.Author);
        createdLecture.Topic.Should().Be(lecture.Topic);
        createdLecture.Description.Should().Be(lecture.Description);
    }
}
