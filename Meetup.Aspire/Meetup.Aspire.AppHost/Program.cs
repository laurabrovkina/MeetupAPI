using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<MeetupAPI>("meetupapi");

builder.Build().Run();